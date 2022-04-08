using AutoMapper;
using ITServiceApp.Extensions;
using ITServiceApp.Models;
using ITServiceApp.Models.Identity;
using ITServiceApp.Models.Services;
using ITServiceApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ITServiceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IEmailSender emailSender , IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            CheckRoles();
            _emailSender = emailSender;
            _mapper = mapper;
        }

        private void CheckRoles()
        {
            foreach (var roleName in RoleNames.Roles)
            {
                if (!_roleManager.RoleExistsAsync(roleName).Result)
                {
                    var result = _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = roleName
                    }).Result;
                }
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Password = string.Empty;
                model.ConfirmPassword = string.Empty;
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user!=null)
            {
                ModelState.AddModelError(nameof(model.UserName), errorMessage: "Bu kullanici adi sisteme daha önce giriş yaptı.");

            }
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.Email), errorMessage: "Bu mail adi sisteme daha önce giriş yaptı.");

            }
            user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                SurName = model.SurName,
            };
            var result=await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded)
            {
                //TODO:kullanıcıya rol atama
                var count = _userManager.Users.Count();
                result = await _userManager.AddToRoleAsync(user,count== 1 ? RoleNames.Admin : RoleNames.User);
                //if (count == 1) //admin
                //{
                //    result = await _userManager.AddToRoleAsync(user, RoleNames.Admin);
                //}
                //else //user
                //{
                //    result = await _userManager.AddToRoleAsync(user, RoleNames.User);
                //}

                //kullanıcıya email doğrulama gönderme
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                    Subject = "Confirm your email"
                };

                await _emailSender.SendAsync(emailMessage);

            }
            else
            {
                ModelState.AddModelError(string.Empty, errorMessage: "Kayıt işleminde bir hata oluştu.");
                return View(model);
            }
            return View();
        }

        public async  Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if(userId == null || code == null)
                return RedirectToAction("Index", "Home");

            var user= await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            ViewBag.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            if (result.Succeeded && _userManager.IsInRoleAsync(user, RoleNames.Passive).Result)
            {
                await _userManager.RemoveFromRoleAsync(user, RoleNames.Passive);
                await _userManager.AddToRoleAsync(user, RoleNames.User);
            }
            return View();
        }
         [HttpGet]
        public IActionResult Login()
        {
         
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = 
                await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe,true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adi veya şifre hatalı");
                return View(model);
            }

        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
       public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            var model = _mapper.Map<UserProfileViewModel>(user);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());
            user.Name= model.Name;
            user.SurName = model.Surname;
            if (user.Email!=model.Email)
            {
                await _userManager.RemoveFromRoleAsync(user, RoleNames.User);
                await _userManager.AddToRoleAsync(user, RoleNames.Passive);
                 user.Email = model.Email;
                user.EmailConfirmed=false;

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                    Subject = "Confirm your email"
                };

                await _emailSender.SendAsync(emailMessage);

            }

            var result= await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, ModelState.ToFullErrorString());
            }

            return View(model);
        }
    }
}
