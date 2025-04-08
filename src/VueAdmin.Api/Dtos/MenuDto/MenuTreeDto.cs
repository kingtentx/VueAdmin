using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace VueAdmin.Api.Dtos
{
    public class MenuTreeDto
    {     

        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 
        /// </summary>      
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>       
        public string Component { get; set; }
        /// <summary>
        /// 
        /// </summary>     
        public MetaDto Meta { get; set; }
        /// <summary>
        /// 
        /// </summary>      
        public List<MenuTreeDto> Children { get; set; }
    }

    /// <summary>
    /// MetaDto
    /// </summary>
    public class MetaDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Sort { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>      
        //public List<string> Roles { get; set; }
        /// <summary>
        /// 
        /// </summary>          
        public List<string> Auths { get; set; }
    }
}
