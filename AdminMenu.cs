using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Arana.VersionManager {
    public class AdminMenu : INavigationProvider {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("Content"),
                        menu => menu.Add(T("Trash"), "100", 
                            item => item.Action("ListDeleted", "Admin", new {area = "Arana.VersionManager", id = ""}).LocalNav()));
        }
    }
}