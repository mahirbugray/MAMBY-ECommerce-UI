﻿using System.ComponentModel.DataAnnotations;

namespace MAMBY.UI.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
