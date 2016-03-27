using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using IndecisionEngine.Models;
using Microsoft.AspNet.Http;

namespace IndecisionEngine.Controllers
{
    // History entries are stored in the session as individual entries with the key "story.history.{id}".
    // The current count is stored in "story.history.count".
    public static class HistoryHelper
    {
        private const string NodeKeyPrefix = "story.history.n.";
        private const string CountKey = "story.history.nodecount";
        private const string SeedIdKey = "story.history.seed";

        public static void AppendToHistory(HttpContext context, StoryTransition transition, string endState)
        {
            var count = context.Session.GetInt32(CountKey);
            var nextId = (count ?? 0) + 1;

            var bytes = Serialize(new HistoryEntry()
            {
                Id = nextId,
                ChoiceId = transition.ChoiceId.Value,
                EndEntryId = transition.NextEntryId.Value,
                EndState = endState,
            });

            context.Session.Set(NodeKeyPrefix + nextId.ToString(CultureInfo.InvariantCulture), bytes);
            context.Session.SetInt32(CountKey, nextId);
        }

        public static int? GetSeedId(HttpContext context)
        {
            return context.Session.GetInt32(SeedIdKey);
        }

        public static IList<HistoryEntry> GetHistory(HttpContext context)
        {
            var historyKeys = context.Session.Keys.Where(key => 
                key.StartsWith(NodeKeyPrefix, System.StringComparison.Ordinal));

            var history = new List<HistoryEntry>();
            foreach (var key in historyKeys)
            {
                byte[] bytes;
                context.Session.TryGetValue(key, out bytes);
                history.Add(Deserialize(bytes));
            }

            var count = context.Session.GetInt32(CountKey);
            Debug.Assert(count == history.Count);

            history = history.OrderBy(entry => entry.Id).ToList();

            return history;
        }

        // Clear all history data and restart with the given seed.
        internal static void Reset(HttpContext context, int seedId)
        {
            var historyKeys = context.Session.Keys.Where(key =>
                key.StartsWith(NodeKeyPrefix, StringComparison.Ordinal)).ToList();

            foreach (var key in historyKeys)
            {
                context.Session.Remove(key);
            }
            context.Session.SetInt32(CountKey, 0);
            context.Session.SetInt32(SeedIdKey, seedId);
        }

        // Pop all later entries off the history and reset the count.
        // Return the matching entry (which should now be the last item).
        public static HistoryEntry GoBackTo(HttpContext context, int id)
        {
            Debug.Assert(id <= context.Session.GetInt32(CountKey));

            // Enumerate history entries, remove all with an Id higher than the given one.

            var historyKeys = context.Session.Keys.Where(key =>
                key.StartsWith(NodeKeyPrefix, StringComparison.Ordinal)).ToList();

            HistoryEntry entry = null;

            foreach (var key in historyKeys)
            {
                var idString = key.Substring(NodeKeyPrefix.Length);
                var nodeId = int.Parse(idString, NumberStyles.None, CultureInfo.InvariantCulture);

                if (nodeId > id)
                {
                    context.Session.Remove(key);
                }

                if (nodeId == id)
                {
                    entry = Deserialize(context.Session.Get(key));
                }
            }

            // Reset the entry count.
            context.Session.SetInt32(CountKey, id);

            Debug.Assert(entry != null);
            return entry;
        }

        // Id, ChoiceId, EndEntryId, State byte Length, State UTF8 string
        private static byte[] Serialize(HistoryEntry entry)
        {
            var buffer = new MemoryStream();
            AppendInt(buffer, entry.Id);
            AppendInt(buffer, entry.ChoiceId);
            AppendInt(buffer, entry.EndEntryId);
            AppendString(buffer, entry.EndState);
            return buffer.ToArray();
        }

        private static void AppendInt(MemoryStream buffer, int value)
        {
            var bytes = new byte[]
            {
                (byte)(value >> 24),
                (byte)(0xFF & (value >> 16)),
                (byte)(0xFF & (value >> 8)),
                (byte)(0xFF & value)
            };
            buffer.Write(bytes, 0, bytes.Length);
        }

        private static void AppendString(MemoryStream buffer, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                AppendInt(buffer, 0);
            }
            else
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                AppendInt(buffer, bytes.Length);
                buffer.Write(bytes, 0, bytes.Length);
            }
        }
        
        // Id, ChoiceId, EndEntryId, State byte Length, State UTF8 string
        private static HistoryEntry Deserialize(byte[] bytes)
        {
            var buffer = new MemoryStream(bytes);
            return new HistoryEntry()
            {
                Id = ReadInt(buffer),
                ChoiceId = ReadInt(buffer),
                EndEntryId = ReadInt(buffer),
                EndState = ReadString(buffer),
            };
        }

        private static int ReadInt(MemoryStream buffer)
        {
            var bytes = new byte[4];
            var read = buffer.Read(bytes, 0, bytes.Length);
            Debug.Assert(read == bytes.Length);
            return bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3];
        }

        private static string ReadString(MemoryStream buffer)
        {
            var length = ReadInt(buffer);
            if (length == 0)
            {
                return string.Empty;
            }
            else
            {
                var bytes = new byte[length];
                var read = buffer.Read(bytes, 0, bytes.Length);
                Debug.Assert(read == bytes.Length);
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}
