using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rp3.Test.Mvc.Controllers
{
    public class PersonController : Controller
    {
        /// <summary>
        /// Es la referencia única al Id de la persona seleccionada en toda la aplicación
        /// </summary>
        public static int? PersonId
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["personId"] != null)
                    return (int)System.Web.HttpContext.Current.Session["personId"];
                return null;
            }
            private set
            {
                if (value == null)
                    System.Web.HttpContext.Current.Session.Remove("personId");
                else
                    System.Web.HttpContext.Current.Session["personId"] = value;
            }
        }

        /// <summary>
        /// Es la referencia única a la listas de personas existentes en toda la aplicación
        /// </summary>
        public static List<Models.PersonViewModel> Persons
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["persons"] != null)
                    return (List<Models.PersonViewModel>)System.Web.HttpContext.Current.Session["persons"];
                return null;
            }
            private set
            {
                if (value == null)
                    System.Web.HttpContext.Current.Session.Remove("persons");
                else
                    System.Web.HttpContext.Current.Session["persons"] = value;
            }
        }

        /// <summary>
        /// Retorna vista con el listado de personas ordenado alfabéticamente
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View(GetAllPersons());
        }

        /// <summary>
        /// Permite cargar los valores en sesión en caso que no se hayan cargado previamente
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult PersonSelection()
        {
            if (Persons == null)
            {
                Persons = GetAllPersons(); ;
            }

            return PartialView("PersonSelectionControl");
        }

        /// <summary>
        /// Permite seleccionar una persona y routear a la página Home, ya sea a través del control en formulario o 
        /// a través de routing con el id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PersonSelected(string id)
        {
            var data = id.Split('-');
            if (int.TryParse(data[0], out int number))
            {
                PersonId = number;
            }
            return RedirectToAction(data[1], data[2]);
        }

        /// <summary>
        /// Permite deseleccionar la persona y volver a la pantalla Home
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonUnselected(string id)
        {
            PersonId=null;
            var data = id.Split('-');
            return RedirectToAction(data[0], data[1]);
        }

        /// <summary>
        /// Obtenemos la lista de personas ordenadas por orden alfabético
        /// </summary>
        /// <returns>Listado de personas</returns>
        private List<Models.PersonViewModel> GetAllPersons()
        {
            Proxies.Proxy proxy = new Proxies.Proxy();

            List<Models.PersonViewModel> persons = proxy.GetPersons().
                Select(p => new Models.PersonViewModel()
                {
                    Active = p.Active,
                    PersonId = p.PersonId,
                    Name = p.Name
                }).ToList();

            return persons.OrderBy(X=>X.Name).ToList();
        }
    }
}