using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CoolabrThird.Models
{
    public class Task
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public DateTime LastUpdated { get; set; }

        public string LastUpdatedFormatted
        {
            get { return string.Format("{0:yyyy-MM-dd HH:mm:ss}", LastUpdated); }
        }

        public string TitleAsMd
        {
            get
            {
                var md = new MarkdownDeep.Markdown
                             {
                                 SafeMode = true,
                                 ExtraMode = true
                             };
                return md.Transform(Title);
            }
        }

        public string UserId { get; set; }
        [JsonIgnore]
        public string UserFullName { get; set; }
    }
}