using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class AssetItems : Items
    {
        public AssetItems()
        {
            ItemType = "itFixedAssets";
        }
    }
}