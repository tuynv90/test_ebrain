﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebrain.admin.bc.Report
{
    public class FeatureGroup
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
