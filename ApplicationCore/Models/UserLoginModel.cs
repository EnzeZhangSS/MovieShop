using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models
{
    public class UserLoginModel
    {
        // The DataType attributes emit HTML 5 data- (pronounced data dash) attributes that HTML 5 browsers can
        // understand. The DataType attributes do not provide any validation
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
