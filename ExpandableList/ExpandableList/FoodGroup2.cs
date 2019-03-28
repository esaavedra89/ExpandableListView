using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace ExpandableList
{
    //Clase base
    public class Food2
    {
        #region Properties
        public string NumeroCaja { get; set; }
        public string NombreTipoEspecie { get; set; }
        public string FechaEntregaAlProductorString { get; set; }
        #endregion

        #region Commands
        public Command CajaCommand
        {
            get
            {
                return new Command(() =>
                {

                    CajaCom();

                });
            }
        }
        #endregion

        #region Methods
        private void CajaCom()
        {
            //Este metodo obtiene las propiedades de las cajas, o en este caso de la parte inferior de la lista
            var numerCaja = this.NumeroCaja;
            var nombreTipoEspecie = this.NombreTipoEspecie;
            var fechaEntregaAlProductorString = this.FechaEntregaAlProductorString;
        } 
        #endregion
    }
    //ViewModel
    public class FoodGroup2 : ObservableCollection<Food2>, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Attributes
        bool _expanded;
        public ObservableCollection<FoodGroup2> all;
        #endregion

        #region Properties
        public string Title { get; set; }
        public string NombreProductor { get; set; }
        public string NumeroMeliponario { get; set; }
        public string FechaCreacionString { get; set; }
        public string TitleWithItemCount
        {
            get
            {
                return string.Format("{0} ({1})", Title, FoodCount);
            }
        }
        public string ShortName { get; set; }
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }

        public string StateIcon
        {
            get { return Expanded ? "expanded_blue.png" : "collapsed_blue.png"; }
        }

        public int FoodCount { get; set; }

        public FoodGroup2(string nombreProductor, string numeroMeliponario, string fechaCreacionString, bool expanded = true)
        {
            this.NombreProductor = nombreProductor;
            this.NumeroMeliponario = numeroMeliponario;
            this.FechaCreacionString = fechaCreacionString;
            this.Expanded = expanded;
        }

        public ObservableCollection<FoodGroup2> All
        {
            //get; private set;
            get { return all; }
            set
            {
                all = value;
                OnPropertyChanged("All");
            }
        }
        #endregion

        #region Constructors
        public FoodGroup2()
        {
            try
            {
                ObservableCollection<FoodGroup2> Groups = new ObservableCollection<FoodGroup2>
            {
                new FoodGroup2("Roger Sanchez", "4", "11/09/1989")
                {
                    new Food2 { NumeroCaja = "1", NombreTipoEspecie = "Mariola", FechaEntregaAlProductorString = "11/09/1989"},
                    new Food2 { NumeroCaja = "2", NombreTipoEspecie = "Jicote gato", FechaEntregaAlProductorString = "29/05/1989"},
                    new Food2 { NumeroCaja = "3", NombreTipoEspecie = "Mariola", FechaEntregaAlProductorString = "11/09/1989"},
                    new Food2 { NumeroCaja = "4", NombreTipoEspecie = "Soncuano", FechaEntregaAlProductorString = "Icon"},
                },
                new FoodGroup2("Francisco Mena", "5", "06/10/1989")
                {
                    new Food2 { NumeroCaja = "5", NombreTipoEspecie = "Jicote gato", FechaEntregaAlProductorString = "29/05/1989"},
                    new Food2 { NumeroCaja = "6", NombreTipoEspecie = "Mariola", FechaEntregaAlProductorString = "29/05/1989"},
                    new Food2 { NumeroCaja = "7", NombreTipoEspecie = "Soncuano", FechaEntregaAlProductorString = "06/10/1989"},
                },
                new FoodGroup2("Nestor Osorio", "6", "13/04/1989")
                {
                    new Food2 { NumeroCaja = "8", NombreTipoEspecie = "Soncuano", FechaEntregaAlProductorString = "11/09/1989"},
                },
                new FoodGroup2("Eleazar Saavedra", "7", "29/05/1989")
                {
                    new Food2 { NumeroCaja = "9", NombreTipoEspecie = "Mariola", FechaEntregaAlProductorString = "06/10/1989"},
                    new Food2 { NumeroCaja = "10", NombreTipoEspecie = "Mariola", FechaEntregaAlProductorString = "11/09/1989"},
                },
            };

                //All = Groups;
                All = new ObservableCollection<FoodGroup2>();
            }
            catch (Exception error)
            {

            }


            Consultar();
        }
        #endregion

        #region Commands
        public Command MeliponariosCommand
        {
            get
            {
                return new Command(() =>
                {

                    MeliponariosCom();

                });
            }
        }
        #endregion

        #region Methods
        private void MeliponariosCom()
        {
            //Este metodo obtiene las propiedades del Meliponaro, o en este caso de la parte superior de la lista
            var nombreProductor = this.NombreProductor;
            var numeroMeliponario = this.NumeroMeliponario;
            var fechaCreacionString = this.FechaCreacionString;
        }

        async void Consultar()
        {
            try
            {

                ObservableCollection<FoodGroup2> meliponarios = new ObservableCollection<FoodGroup2>();

                //List<Meliponario> ListaMeliponarioSub = new List<Meliponario>();
                List<MeliponarioCaja> cajaLista = new List<MeliponarioCaja>();
                var Identificador = "7826dd2a-8c21-49e3-b8fc-5555d53278c0";
                var Tabla2 = "Meliponario";
                var Client = new HttpClient();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("Application/json"));
                HttpResponseMessage response2 = await Client.GetAsync(
                "http://neoapi.neotecnologias.com/floranueva/api/SeleccionIdentificador?NombreTabla=" + Tabla2 + "&Identificador=" + Identificador + "&Id=0");
                if (!response2.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response2.RequestMessage.ToString(),
                    "Aceptar");

                    return;
                }

                var result2 = response2.Content.ReadAsStringAsync().Result;
                var resulta = JsonConvert.DeserializeObject<string>(result2);
                if (resulta == "-109") //si (result == -102)---> no existen registros
                {
                    return;
                }
                else if (resulta != "-102") //si (result == -102)---> no existen registros
                {
                    var TodosMelipon = JsonConvert.DeserializeObject<List<Meliponario>>(resulta);
                    var TodosMeliponarios = TodosMelipon.Where(l => l.Estado == "ACTIVO").ToList();

                    foreach (Meliponario item2 in TodosMeliponarios)
                    {

                        var meliponario = new FoodGroup2(item2.CodigoFloraNueva, item2.NumeroMeliponario.ToString(), item2.FechaCreacion.ToString());

                        var Client2 = new HttpClient();
                        Client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("Application/json"));
                        HttpResponseMessage response22 = await Client2.GetAsync(
                            "http://neoapi.neotecnologias.com/floranueva/api/SeleccionCajasMeliponariosPorId?IdMeliponario=" + item2.IdMeliponario);
                        if (!response22.IsSuccessStatusCode)
                        {
                            await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            response22.RequestMessage.ToString(),
                            "Aceptar");

                            return;
                        }

                        var result23 = response22.Content.ReadAsStringAsync().Result;
                        var resultaCaja = JsonConvert.DeserializeObject<string>(result23);

                        if (resultaCaja == "-109") //si (result == -102)---> no existen registros
                        {
                            return;
                        }
                        else if ((resultaCaja != "-102") && (resultaCaja != "-101")) //si (result == -102)---> no existen registros
                        {

                            var ListaCajas = JsonConvert.DeserializeObject<List<MeliponarioCaja>>(resultaCaja);
                            List<MeliponarioCaja> cajass = ListaCajas.Where((i => i.IdMeliponario == item2.IdMeliponario)).ToList();
                            List<MeliponarioCaja> MisCajass = cajass.Where(i => i.Estado == "ACTIVO").ToList();

                            var DataCajas = MisCajass;

                            foreach (var elements in DataCajas)
                            {
                                cajaLista.Add(new MeliponarioCaja
                                {
                                    NumeroCaja = elements.NumeroCaja,
                                    NombreTipoEspecie = string.Empty,
                                    FechaEntregaAlProductor = elements.FechaEntregaAlProductor,
                                    FechaEntregaAlProductorString = string.Empty,
                                    ActivaYesNot = "Si",
                                    Comentario = elements.Comentario,
                                    Activa = elements.Activa,
                                    IdEspecieAbeja = elements.IdEspecieAbeja,
                                    Dispositivo = elements.Dispositivo,
                                    Estado = elements.Estado,
                                    FechaCreacion = elements.FechaCreacion,
                                    FechaCreacionUtc = elements.FechaCreacionUtc,
                                    FechaModificacion = elements.FechaModificacion,
                                    FechaModificacionUtc = elements.FechaModificacionUtc,
                                    Identificador = elements.Identificador,
                                    IdMeliponario = elements.IdMeliponario,
                                    IdMeliponarioCaja = elements.IdMeliponarioCaja,
                                    IdMeliponarioCajaLocal = elements.IdMeliponarioCajaLocal,
                                    IdMeliponarioLocal = 0,
                                    IdOrigenColonia = elements.IdOrigenColonia,
                                    Usuario = elements.Usuario,
                                });

                                meliponario.Add(new Food2()
                                {
                                    NumeroCaja = elements.NumeroCaja.ToString(),
                                    NombreTipoEspecie = elements.IdEspecieAbeja.ToString(),
                                    FechaEntregaAlProductorString = elements.FechaEntregaAlProductor.ToString()
                                });
                            }

                            meliponarios.Add(meliponario);

                            var cajasAN = new List<MeliponarioCaja>(cajaLista);

                            var CajasAN = cajasAN.OrderBy(l => l.NumeroCaja).ToList();

                            var MeliponariocajaJson = JsonConvert.SerializeObject(CajasAN);
                        }

                        //ListaMeliponarioSub.Add(new Meliponario
                        //{
                        //    NombreProductor = string.Empty,
                        //    NumeroMeliponario = item2.NumeroMeliponario,
                        //    FechaCreacion = item2.FechaCreacion,
                        //    IdMeliponario = item2.IdMeliponario,
                        //    IdMeliponarioLocal = item2.IdMeliponarioLocal,
                        //    Identificador = item2.Identificador,
                        //    CodigoFloraNueva = item2.CodigoFloraNueva,
                        //    GPSLatitud = item2.GPSLatitud,
                        //    GPSLongitud = item2.GPSLongitud,
                        //    IdProductor = item2.IdProductor,
                        //    IdTipoMeliponario = item2.IdTipoMeliponario,
                        //    NombreTipoMeliponario = item2.NombreTipoMeliponario,
                        //    Observaciones = item2.Observaciones,
                        //    Usuario = "USUARIO",
                        //    ValidadoParaRecibirCajasFloraNueva = item2.ValidadoParaRecibirCajasFloraNueva,
                        //    FechaCreacionString = item2.FechaCreacion.ToString(),
                        //    FechaCreacionMeliponario = item2.FechaCreacion,
                        //    ListaMeliponarioCaja = cajaLista
                        //});
                    }
                    All = meliponarios;
                }
            }
            catch (Exception error)
            {

            }
        } 
        #endregion
    }
    //Clase para deserializar
    public class Meliponario
    {
        #region Properties
        public int IdMeliponarioLocal { get; set; } //agregado para uso interno bdLocal
        public int IdMeliponario { get; set; }
        public int NumeroMeliponario { set; get; }
        public string Identificador { get; set; }
        public int IdPrincipalLocal { get; set; }
        public int IdProductor { get; set; }
        public double? GPSLatitud { get; set; }
        public double? GPSLongitud { get; set; }
        public bool ValidadoParaRecibirCajasFloraNueva { get; set; }
        public int? IdTipoMeliponario { set; get; }
        public DateTime FechaCreacion { get; set; }
        public string FechaCreacionString { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public string Usuario { get; set; }
        public string Dispositivo { get; set; }
        public string FechaCreacionUtc { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string FechaModificacionUtc { get; set; }
        public string Transaccion { get; set; }
        public string CodigoFloraNueva { get; set; }
        public DateTime FechaCreacionMeliponario { get; set; }
        public string NombreTipoMeliponario { get; set; }
        public string NombreProductor { get; set; }
        public List<MeliponarioCaja> ListaMeliponarioCaja { get; set; }
        #endregion
    }
    //Clase para deserializar
    public class MeliponarioCaja
    {
        #region Properties
        public int IdMeliponarioCajaLocal { get; set; }
        public int IdMeliponarioCaja { get; set; }
        public int NumeroCaja { get; set; }
        public string Identificador { get; set; }
        public int IdMeliponarioLocal { get; set; }
        public int IdMeliponario { get; set; }
        public int? IdEspecieAbeja { get; set; }
        public DateTime FechaEntregaAlProductor { get; set; }
        public string FechaEntregaAlProductorString { get; set; }
        public string Comentario { get; set; }
        public int? IdOrigenColonia { get; set; }
        public bool Activa { get; set; }
        public string Estado { get; set; }
        public string Usuario { get; set; }
        public string Dispositivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string FechaCreacionUtc { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string FechaModificacionUtc { get; set; }
        public string Transaccion { get; set; }
        public string NombreTipoEspecie { set; get; }
        public string ActivaYesNot { set; get; }
        public bool Checked { get; set; }
        public bool Checkado { get; set; }
        #endregion
    }
}
