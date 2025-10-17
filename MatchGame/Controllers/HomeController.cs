using AutoMapper;
using MatchGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MatchGame.Controllers
{
    public class HomeController : Controller
    {
        // Модель.
        DataRecordEntities context = new DataRecordEntities();
        private static MatchModel _matchModel;
        private static IMapper _mapper;
        // Имя играющего сейчас пользователя.
        private static string _nameUser;

        public ActionResult Index()
        {
            int numberField = Convert.ToInt32(new XML().LoadAttributs("numberField.xml")[0]);

            Book book = new Book() { Name = "kol" };

            _matchModel = new MatchModel();
            _matchModel.initGame(numberField);

            List<int> list = new List<int>() { 5, 7, 8, 90, 77 };
            var kol = Where(list, Test).ToList();
            // _mapper = new 
            //var person = _mapper.Map<BookView>(book);

            return View();
        }

        bool Test(int i) {
            if (i > 9) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Таблица рекордов.
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            var ListReg = (from a in context.TableRecord select a).ToList();

            return View(ListReg);
        }

        public ActionResult Contact()
        {
            return View();
        }


        /// ////////////////////////////////////////////////////////
        public class MathField
        {
            public int countPlayer { get; set; }
            public int sizeField { get; set; }
            public int[,] gameField { get; set; }
        }
        /// <summary>
        /// Дать View обновленную таблицу.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AjaxLaunch()
        {
            //обновить таблицу рекордов.
            try
            {
                var card = (from a in context.TableRecord where a.name == _nameUser orderby a.Id descending select a).FirstOrDefault();
                card.score = MatchModel.scorePlayer;
                UpdateModel(card);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error AjaxLaunch" + ex);

            }
            // Отправляем модель игровое поле с изменными условиями.
            return Json(new MathField()
            {
                countPlayer = MatchModel.scorePlayer,
                sizeField = MatchModel.sizeField,
                gameField = MatchModel.gameField
            });

        }
        /// <summary>
        /// Клик игрока по карточке поля.
        /// </summary>
        /// <param name="intStr"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AjaxClickTile(string intStr)
        {
            //Подготовка - обратное преобразование массива.
            int x = Convert.ToInt32(intStr) / MatchModel.sizeField;
            int y = Convert.ToInt32(intStr) % MatchModel.sizeField;

            _matchModel.clickGameField(x, y);

            return Json(" жмякнули = " + intStr);

        }
        /// <summary>
        /// Получение имени игрока.
        /// </summary>
        /// <param name="nameStr"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AjaxNameUser(string nameStr)
        {
            _nameUser = nameStr;
            try
            {
                TableRecord card = new TableRecord() { name = nameStr, score = 0 };
                context.TableRecord.Add(card);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error " + ex);
                return Json(" error " + ex);
            }
            return Json(" ок");

        }
        [HttpPost]
        public JsonResult NumberField(string numStr)
        {
            new XML().SaveAttributs("NumberField.xml", numStr);
            return Json(" ок");
        }

        public IEnumerable<int> Where(IEnumerable<int> list, Func<int,bool> func) {
            foreach (var item in list)
            {
                if (func(item))
                {
                    yield return item;
                }
            }
        }

    }
}