using System.ComponentModel.DataAnnotations;

namespace Zrk.Common.Paged
{
    public class PagedInputDto
    {
        [Range(1, 50)]
        public int Pagesize { get; set; } = 5;

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public bool Descending { get; set; }

        public dynamic Filter { get; set; }

        private string _SortBy;
        public string SortBy
        {
            get
            {
                return this._SortBy;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = "LastModificationTime";
                }
                this._SortBy = value;
            }
        }
    }
}
