using Newtonsoft.Json;
using System.Net.Http.Json;
using U3ActRegistroDeActividadesMaui.Models.DTOs;

namespace U3ActRegistroDeActividadesMaui.Services
{
    public class DepartamentosService
    {
        HttpClient client = new();

        Repositories.DepartamentosRepository departamentosRepository = new();
        public DepartamentosService()
        {
            //Este link puede cambiar
            client.BaseAddress = new Uri("http://localhost:55555/");
        }

        public event Action? DatosActualizadosDep;


        public async Task GetDepartamentos()
        {
            try
            {
                var fecha = Preferences.Get("UltimaFechaActualizacion", DateTime.MinValue);

                bool aviso = false;

                var response = await client.GetFromJsonAsync<List<DepartamentoDTO>>($"/Departamentos/{fecha:yyyy-MM-dd}/{fecha:HH}/{fecha:mm}");

                if (response != null)
                {
                    foreach (DepartamentoDTO departamento in response)
                    {
                        var entidad = departamentosRepository.Get(departamento.Id);

                        if(entidad == null)
                        {
                            entidad = new()
                            {
                                Id = entidad.Id,
                                Username = entidad.Username

                            };
                            departamentosRepository.Insert(entidad);
                            aviso = true;
                        }
                        else
                        {
                            if(entidad != null)
                            {
                                
                            }
                        }

                        if (aviso)
                        {

                            _ = MainThread.InvokeOnMainThreadAsync(() =>
                            {
                                DatosActualizadosAct?.Invoke();
                            });
                        }

                        //Preferences.Set("UltimaFechaActualizacion", response.Max(x => ));

                    }
                }
            }
            catch
            {

            }
        }

        public async Task<IEnumerable<DepartamentoDTO>?>? GetAll(DepartamentoDTO dto)
        {
            try
            {
                //Hacer la peticion a la api
                var response = await client.GetAsync("/Departamentos");
                if (response.IsSuccessStatusCode)
                {
                    //convertir el body en json
                    var json = await response.Content.ReadAsStringAsync();
                    //convertir el json en lista
                    var result = JsonConvert.DeserializeObject<IEnumerable<DepartamentoDTO>>(json);
                    //regresar lista de departamentos
                    return result;
                }
            }
            catch (Exception ex)
            {
                //Muestra posibles errores como mensaje en pantalla
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            return null;
        }
        public async Task<DepartamentoDTO?> Get(DepartamentoDTO dto)
        {
            try
            {
                var response = await client.GetAsync($"/Departamento/{dto.Id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<DepartamentoDTO>(json);
                    return result;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
            return null;
        }
        public async Task Insert(DepartamentoDTO dto)
        {
            try
            {
                var response = await client.PostAsJsonAsync("/Agregar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        public async Task Update(DepartamentoDTO dto)
        {
            try
            {
                var response = await client.PutAsJsonAsync("/Editar", dto);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        public async Task Delete(DepartamentoDTO dto)
        {
            try
            {
                var response = await client.DeleteAsync($"/Eliminar/{dto.Id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
    }
}
