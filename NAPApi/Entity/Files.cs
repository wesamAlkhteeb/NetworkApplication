﻿using System.ComponentModel.DataAnnotations;

namespace NAPApi.Entity
{
    public class Files
    {
        public int FilesId { get; set; }
        [MaxLength(60)]
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int FileIdUses { get; set; }
        public DateTime FileCreateDate { get; set; }

        //navigator

        public List<Report> reports { set; get; }
        public int GroupId { get; set; }
        public Group Groups { set; get; }

    }
}
