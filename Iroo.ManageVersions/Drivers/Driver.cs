using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Iroo.ManageVersions.Drivers {
    public class Driver : ContentPartDriver<ContentPart> {
        protected override DriverResult Display(ContentPart part, string displayType, dynamic shapeHelper) {
            if (displayType == "SummaryAdmin") {
                return ContentShape("Parts_ManageVersions_SummaryAdmin",
                                    () => shapeHelper.Parts_ManageVersions_SummaryAdmin(ContentPart: part));
            }
            return null;
        }
    }
}