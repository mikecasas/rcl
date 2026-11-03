// AuthModel.cs
using System.ComponentModel.DataAnnotations;

namespace WasmBaseServices.Models;

public class LoginResult
{
	public string? AccessToken { get; set; }

	public string? RefreshToken { get; set; }

	public bool IsSuccess { get; set; }

        public string? DefaultPage { get; set; }
    }

public class LoginModel
{
	[Required(ErrorMessage = "Email is required.")]
	//[EmailAddress(ErrorMessage = "Email address is not valid.")]
	public string? UserName { get; set; } // NOTE: email will be the username, too

	[Required(ErrorMessage = "Password is required.")]
	[DataType(DataType.Password)]
	public string? Password { get; set; }
	
	//[EmailAddress(ErrorMessage = "Email address is not valid.")]
	public string? PrimaryEmailAddress { get; set; } 

}


public class RegModel : LoginModel
{
	[Required(ErrorMessage = "Confirm password is required.")]
	[DataType(DataType.Password)]
	[Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
	public string confirmpwd { get; set; }
}