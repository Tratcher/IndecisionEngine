﻿using System.Linq;
using System.Threading.Tasks;
using IndecisionEngine.Models;
using IndecisionEngine.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IndecisionEngine.Controllers
{
    [Authorize("Admin")]
    [RequireHttps]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public AccountsController(UserManager<ApplicationUser> userManager, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<AccountsController>();
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        // GET: /<controller>/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            var viewModel = new AccountsDetailsViewModel(user)
            {
                Logins = await _userManager.GetLoginsAsync(user),
                Claims = await _userManager.GetClaimsAsync(user),
            };

            return View(viewModel);
        }

        // GET: /<controller>/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var viewModel = new AccountsDetailsViewModel(user)
            {
                Logins = await _userManager.GetLoginsAsync(user),
                Claims = await _userManager.GetClaimsAsync(user),
            };
            return View(viewModel);
        }

        // POST: /<controller>/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountsDetailsViewModel viewModel)
        {
            ApplicationUser user;
            if (ModelState.IsValid)
            {
                user = await _userManager.FindByIdAsync(viewModel.Id);
                if (user == null)
                {
                    return NotFound();
                }
                user.DisplayName = viewModel.DisplayName;
                user.UserName = viewModel.UserName;
                // TODO: Option to re-send validation e-mail.
                user.EmailConfirmed = viewModel.EmailConfirmed;
                user.PhoneNumber = viewModel.PhoneNumber;
                var result = await _userManager.UpdateAsync(user);
                AddErrors(result);
            }

            user = await _userManager.FindByIdAsync(viewModel.Id);
            if (user == null)
            {
                return NotFound();
            }
            viewModel = new AccountsDetailsViewModel(user)
            {
                Logins = await _userManager.GetLoginsAsync(user),
                Claims = await _userManager.GetClaimsAsync(user),
            };
            return View(viewModel);
        }

        // GET: /<controller>/Delete/5
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var viewModel = new AccountsDetailsViewModel(user)
            {
                Logins = await _userManager.GetLoginsAsync(user),
                Claims = await _userManager.GetClaimsAsync(user),
            };

            return View(viewModel);
        }

        // POST: /<controller>/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User account {UserName} deleted.", user.UserName);
                return RedirectToAction("Index");
            }
            AddErrors(result);
            var viewModel = new AccountsDetailsViewModel(user)
            {
                Logins = await _userManager.GetLoginsAsync(user),
                Claims = await _userManager.GetClaimsAsync(user),
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            foreach (var user in _userManager.Users.ToList())
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation(2, "User account {UserName} deleted.", user.UserName);
                }
                else
                {
                    _logger.LogInformation(3, "Failed to delete user account {UserName}.", user.UserName);
                }
            }
            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
