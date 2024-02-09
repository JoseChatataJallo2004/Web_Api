using ClienteWebApiCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace ClienteWebApiCore.Controllers
{
    public class MedicosController : Controller
    {

        //GET 
        public async Task<ActionResult> ListarCitasMedico(string codmed)
        {
            //traer la lista de metodos para guardarlos en un viewbag y utilziarlo  para pobras en un dropdwonlist
            var listado = new List<Medicos>();
            //permite realizar una solicitud a un servicio
            using (var httpcliente = new HttpClient())
            {
                //realizamos la consulta get 
                var respuesta = await httpcliente.GetAsync("http://192.168.56.1:7272/api/MedicosAPI/GetMedicos");

                //convertir a cadena el contenido de respuesta 
                string respuestaApi = await respuesta.Content.ReadAsStringAsync();
                //deserializar la cadena   a list<medicos> "listado"
                listado = JsonConvert.DeserializeObject<List<Medicos>>(respuestaApi);

            }

            ViewBag.medicos=new SelectList(listado,"codmed","nommed");

            //citas por medico
            var lista_citas=new List<PA_CITAS_MEDICO>();
            if (codmed != null) { 

                using (var httpcliente=new HttpClient())
                {

                var respuesta = 
                    await httpcliente.GetAsync($"http://192.168.56.1:7272/api/MedicosAPI/GetCitasMedicos/{codmed}");

                //
                string respuestaAPI=await respuesta.Content.ReadAsStringAsync();
                //
                lista_citas = JsonConvert.DeserializeObject<List<PA_CITAS_MEDICO>>(respuestaAPI);
                }
            }
            ViewBag.cantidad = lista_citas?.Count;

            return View(lista_citas);

        }



        // GET: MedicosController
        public async Task<ActionResult> listaMedicos()
        {
            var listado=new List<Medicos>();


            //permite realizar una solicitud a un servicio
            using(var httpcliente=new HttpClient())
            {
                //realizamos la consulta get 
                var respuesta =await  httpcliente.GetAsync("http://192.168.56.1:7272/api/MedicosAPI/GetMedicos");

                //convertir a cadena el contenido de respuesta 
                string respuestaApi=await respuesta.Content.ReadAsStringAsync();
                //deserializar la cadena   a list<medicos> "listado"
                listado = JsonConvert.DeserializeObject<List<Medicos>>(respuestaApi);

            }

            return View(listado);
        }

        // GET: MedicosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MedicosController/Create
        public ActionResult GrabarMedico()
        {
            Medicos nuevo=new Medicos();    
            return View(nuevo);
        }

        // POST: MedicosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>  GrabarMedico(Medicos obj)
        {
            try
            {
                   using (var httpcliente=new HttpClient())
                {
                    obj.eliminado = "No";
                    StringContent contenido = new StringContent(
                        
                        JsonConvert.SerializeObject(obj),Encoding.UTF8,
                        "application/json"

                        );

                    var respuesta = await httpcliente.PostAsync("http://192.168.56.1:7272/api/MedicosAPI/PostMedico", contenido);
                    //
                    string respuestaApi=await respuesta.Content.ReadAsStringAsync();

                    ViewBag.Mensaje = respuestaApi;

                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
            }
            return View(obj);

        }

        // GET: MedicosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MedicosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(listaMedicos));
            }
            catch
            {
                return View();
            }
        }

        // GET: MedicosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MedicosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(listaMedicos));
            }
            catch
            {
                return View();
            }
        }
    }
}
