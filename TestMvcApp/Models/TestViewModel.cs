using System.ComponentModel;

namespace TestMvcApp.Models
{
    public class TestViewModel
    {
        public string Name { get; set;  }

        [DisplayName("Last name")]
        public string Lastname { get; set; }

        public string Description { get; set; }
    }
}