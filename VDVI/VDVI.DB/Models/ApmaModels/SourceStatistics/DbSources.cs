﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[Sources]")]
    public class DbSources : Audit
    {
        public string PropertyCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Listorder { get; set; }
        public bool? Visible { get; set; }
        public string SourceGroup { get; set; } 

    }
}
