using System.Linq;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.UI.Notify;

namespace Iroo.VersionManager.Controllers {
    public class AdminController : Controller {
        public AdminController(IOrchardServices services, IShapeFactory shapeFactory) {
            Services = services;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        public IOrchardServices Services { get; private set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        public ActionResult List(int id) {
            var query = Services.ContentManager.GetAllVersions(id).ToList();
            if (!query.Any())
                return HttpNotFound();

            return View(query);
        }

        public ActionResult ListDeleted() {
            //TODO: This is clearly very inefficient
            var allItems = Services.ContentManager.Query(VersionOptions.AllVersions).List().GroupBy(item => item.Id);
            var removedItems = allItems.Where(g => !g.Any(item => item.VersionRecord.Latest)).Select(g => g.OrderBy(item => item.Version).Last());

            return View(removedItems);
        }


        public ActionResult SetPublishedVersion(int id) {
            var contentItem = Services.ContentManager.Get(-1, VersionOptions.VersionRecord(id));
            if (contentItem == null)
                return HttpNotFound();

            Services.ContentManager.Publish(contentItem);
            Services.Notifier.Add(NotifyType.Information, T("Version {0} of content item published.", contentItem.Version));
            return RedirectToAction("List", new {contentItem.Id});
        }

        public ActionResult UnsetPublishedVersion(int id) {
            var contentItem = Services.ContentManager.Get(-1, VersionOptions.VersionRecord(id));
            if (contentItem == null)
                return HttpNotFound();

            Services.ContentManager.Unpublish(contentItem);
            Services.Notifier.Add(NotifyType.Information, T("Version {0} of content item un-published.", contentItem.Version));
            return RedirectToAction("List", new { contentItem.Id });
        }

        public ActionResult Delete(int id) {
            var contentItem = Services.ContentManager.Get(-1, VersionOptions.VersionRecord(id));
            if (contentItem == null)
                return HttpNotFound();

            Services.ContentManager.Remove(contentItem);
            Services.Notifier.Add(NotifyType.Information, T("Content item {0} has been deleted.", contentItem.Id));
            return RedirectToAction("ListDeleted");
        }

        public ActionResult Undelete(int id) {
            var contentItem = Services.ContentManager.Get(-1, VersionOptions.VersionRecord(id));
            if (contentItem == null)
                return HttpNotFound();

            contentItem.VersionRecord.Latest = true;
            Services.Notifier.Add(NotifyType.Information, T("Content item {0} has been un-deleted.", contentItem.Id));
            return RedirectToAction("List", new { contentItem.Id });
        }
    }
}
