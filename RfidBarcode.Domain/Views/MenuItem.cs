namespace RfidBarcode.Domain.Models.Views
{
    public class MenuItem
    {
        public string Label { get; set; } = null!;
        public string? Icon { get; set; }

        public string? Href { get; set; }

        public Dictionary<string, string> Badges { get; set; } = null!;
        public List<MenuItem> ChildMenuItems { get; set; } = new List<MenuItem>();

        public MenuItem()
        {
            Badges = new Dictionary<string, string>();
        }

        private bool CheckActive(string path, string href)
        {
            path = path.Substring(0, path.LastIndexOf('/')) + "/";
            if (path.CompareTo(href) == 0)
            {
                return true;
            }

            return false;

        }
        public string IsActive(string path, string basePath)
        {
            if (CheckActive(basePath + path, Href ?? ""))
            {
                return "active";
            }

            return "";
        }

        public string IsShow(string path, string basePath)
        {
            foreach (var child in ChildMenuItems)
            {
                if (CheckActive(basePath + path, child.Href ?? ""))
                {
                    return "show";
                }
            }

            return "";
        }
    }
}
