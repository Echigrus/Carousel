using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Microsoft.Edge.SeleniumTools;

namespace Mono
{
    public partial class Mono : Form
    {
        static string filename;
        static TimeSpan runtime;
        static DateTime startTime, finishTime;

        static DataGridViewColumn newCell;
        static DataGridViewCell cell;
        static IWebDriver Browser;
        static bool browserInit = false;
        static bool init = false;
        static string url;
        bool tasks = true;
        bool fineLogin = true;
        bool side = true; //true - закон, false - братва
        bool repair = true;
        bool robbery = false;
        bool bank = false;
        bool whitelist = true;
        int precCut = -1;
        static int selector = 0;

        byte grayItem = 1; //{ "Оставить", "Продать" }
        byte specialItem = 2; //{ "Оставить", "Продать", "Общак" }
        String specialList;
        int timing = 400; //0 - по загрузке страницы, остальное с задержкой

        static List<string> taskList = new List<string>();
        static List<string> whiteL = new List<string> { "Бинокль", "Набор медика", "Патроны разрывные 2", "Шприц-пистолет \"Кобра\" 10мл", "Электрошокер", "Ящик боеприпасов XXL", "Прицел 1 ур.", "Подклад 1 ур.", "Пластины 1 ур.", "Механизм 1 ур.", "Ствол 1 ур.", "Прицел 2 ур.", "Подклад 2 ур.", "Пластины 2 ур.", "Механизм 2 ур.", "Ствол 2 ур." };
        static List<string> missions = new List<string> { "Бар \"Зелень\"", "Администрация Вокзала", "Усмирение тигра", "Заброшенный Завод" };
        static List<string> logins = new List<string>();
        static List<string> passwords = new List<string>();

        static void CrashHandler(object sender, UnhandledExceptionEventArgs args)
        {
            if (Browser != null) Browser.Quit();

            filename = "log";
            filename += String.Format("{0}_{1}_{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + ".txt";
            if (!File.Exists(filename))
            {
                File.CreateText(filename);
            }
            Exception e = (Exception) args.ExceptionObject;
            using (StreamWriter sw = File.AppendText(filename))
            {
                sw.WriteLine("ERROR caught: " + e.Message);
                sw.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            }
        }

        public Mono()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(CrashHandler);

            InitializeComponent();

            //statusLabel.Text = "Статус: выбор настроек";

            List<string> browsers = new List<string> { "Chrome", "Firefox", "Edge" };
            browserSelect.Items.AddRange(browsers);
            browserSelect.SelectedIndex = 0; //окошко выбора браузера

            List<string> gray = new List<string> { "Оставить", "Продать" };
            graySelect.Items.AddRange(gray);
            graySelect.SelectedIndex = 1;
            specialBox.Text = "Кредиты, Жетон, Пули";

            List<string> special = new List<string> { "Оставить", "Продать", "Общак" };
            specialSelect.Items.AddRange(special);
            specialSelect.SelectedIndex = 2;

            timingLabel.Text = "Задержка: 1000";
            stop.Enabled = false;

            string[] lines = System.IO.File.ReadAllLines("logpass.txt");
            for (int i = 0; i < lines.Length; i = i + 2)
            {
                logins.Add(lines[i]);
                passwords.Add(lines[i + 1]);
            }

            logpassGrid.AutoGenerateColumns = false;
            newCell = new DataGridViewColumn();
            cell = new DataGridViewTextBoxCell();
            newCell.CellTemplate = cell;
            newCell.FillWeight = 1;
            int defheight = 20;
            logpassGrid.Columns.Add(newCell);
            logpassGrid.ColumnCount = 2;
            logpassGrid.Rows.Add(lines.Length/2);
            for (int i = 0; i < logins.Count; i++)
            {
                logpassGrid.Rows[i].Height = defheight;
                logpassGrid.Rows[i].Cells[0].Value = logins[i];
                logpassGrid.Rows[i].Cells[1].Value = passwords[i];
            }
            logpassGrid.Columns[0].Width = 110;
            logpassGrid.Columns[1].Width = 110;

            start_Click(null, null);
        }

        //fine
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) { if (Browser != null) Browser.Quit(); }

        //fine
        private void BarScrolled(object sender, System.EventArgs e)
        {
            timingLabel.Text = "Задержка: " + timingBar.Value;
        }

        //fine
        private void stop_Click(object sender, EventArgs e)
        {
            BW.CancelAsync();
            init = false;
            selector = 0;
            if (Browser != null)
            {
                Browser.Quit();
                Browser = null;
            }
            taskList.Clear();
            start.Enabled = true;
            stop.Enabled = false;
        }

        //fine
        private void start_Click(object sender, EventArgs e)
        {
            if (logins.Count == passwords.Count && logins.Count != 0) init = true;
            if (init)
            {
                //statusLabel.Text = "Заполнение настроек";
                GetOptions();
                filename = "log";
                filename += String.Format("{0}_{1}_{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year) + ".txt";
                if (!File.Exists(filename))
                {
                    File.CreateText(filename);
                }
                using (StreamWriter sw = File.AppendText(filename))
                {
                    sw.WriteLine("Logins: " + logins.Count + ", timing:" + timing + ", launched at: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                }
                BW.RunWorkerAsync();
                start.Enabled = false;
                stop.Enabled = true;
            }
            else
            {
                Browser.Quit();
            }
        }

        //fine
        private void InitPush()
        {
            string temp;
            fineLogin = true;
            IWebElement logElem = null;
            if (!browserInit)
            {
                if (browserSelect.SelectedIndex == 0)
                {
                    Browser = new OpenQA.Selenium.Chrome.ChromeDriver();
                }
                else if (browserSelect.SelectedIndex == 1)
                {
                    Browser = new OpenQA.Selenium.Firefox.FirefoxDriver();
                }
                else if (browserSelect.SelectedIndex == 2)
                {
                    Browser = new EdgeDriver();
                }
                if (timing <= 400) Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(400);
                Browser.Manage().Window.Position = new Point(0, 0);
            
                url = "http://bratki.mobi";
                browserInit = true;
            }
            Browser.Navigate().GoToUrl(url);
            Thread.Sleep(timing);
            logElem = Browser.FindElement(By.ClassName("btn-login"));
            logElem.Click();
            Thread.Sleep(timing);
            logElem = Browser.FindElement(By.Id("login"));
            logElem.SendKeys(logins[selector]);
            Thread.Sleep(timing);
            logElem = Browser.FindElement(By.Id("password"));
            logElem.SendKeys(passwords[selector]);
            Thread.Sleep(timing);
            Browser.FindElements(By.ClassName("btn-a"))[0].Click();
            Thread.Sleep(timing);
            try
            {
                logElem = Browser.FindElement(By.ClassName("header-resources"));
                Browser.Navigate().GoToUrl(url + "/user");
                Thread.Sleep(timing);
                temp = Browser.FindElements(By.ClassName("user-info"))[0].FindElements(By.TagName("span"))[0].Text;
                if (temp == "Законники") side = true;
                else side = false;
            }
            catch (NoSuchElementException) { fineLogin = false; }
            if (fineLogin)
            {
                //statusLabel.Text = "OK";
                Thread.Sleep(timing);
                init = true;
                Browser.Manage().Window.Size = new Size(520, 940);
            }
            else
            {
                Console.WriteLine("Статус: неправильный логин/пароль");
                //statusLabel.Text = "Статус: неправильный логин/пароль";
            }
        }

        //нет доступа к интерфейсу у всего потока
        private void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!BW.CancellationPending)
            {
                TaskManager();
            }
        }

        //fine
        async private void TaskManager()
        {
            if (taskList.Count == 0)
            {
                startTime = DateTime.Now;
                InitPush();
                if (init) SetTasks();
            }

            while (init && taskList.Count != 0)
            {
                CheckOverfill();
                if (taskList[0].Contains("#robbery:")) DoRobbery();
                else if (taskList[0].Contains("#district:")) GoDistrict();
                else if (taskList[0].Contains("#tasks:")) DoTasks();
                else if (taskList[0].Contains("#missions:")) DoMissions();
                else if (taskList[0].Contains("#items")) DealWithItems();
                else if (taskList[0].Contains("#bank")) Bank();
                taskList.RemoveAt(0);
            }
            if (taskList.Count == 0 || !init) LogOut();
        }

        //fine
        private void CheckOverfill()
        {
            IWebElement telem, ts;
            bool of = false;
            try
            {
                telem = Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("notice-inner"));
                try
                {
                    ts = telem.FindElements(By.TagName("div"))[0];
                    if (ts.Text == "В твоей сумке не хватает места!") of = true;
                    else Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("b-notice-close")).Click();
                }
                catch (NoSuchElementException)
                {
                    try
                    {
                        Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("b-notice-close")).Click();
                    }
                    catch (NoSuchElementException) { }
                }
                catch (ArgumentOutOfRangeException)
                {
                    try
                    {
                        Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("b-notice-close")).Click();
                    }
                    catch (NoSuchElementException) { }
                }
            }
            catch (NoSuchElementException) { }
            if (of) DealWithItems();
        }

        //fine
        private void DealWithItems()
        {
            //statusLabel.Text = "Статус: очистка сумки";
            bool done, tdone, fine;
            string tempstr;
            IWebElement telem;
            int i, lim = 0, k, tlim;
            if (timing != 0) Thread.Sleep(timing);


            if (specialItem == 2)
            { //общак

                IWebElement tel;
                done = false;
                Browser.Navigate().GoToUrl(url + "/gang");
                if (timing != 0) Thread.Sleep(timing);
                Browser.FindElement(By.ClassName("list-btns")).FindElements(By.ClassName("btn-flat"))[2].Click();
                if (timing != 0) Thread.Sleep(timing);
                Browser.FindElements(By.ClassName("b-content"))[0].FindElement(By.ClassName("btn-a")).Click();
                if (timing != 0) Thread.Sleep(timing);
                Browser.FindElements(By.ClassName("b-content"))[1].FindElement(By.TagName("a")).Click();
                if (timing != 0) Thread.Sleep(timing);

                if (!whitelist)
                {
                    while (!done)
                    {
                        try
                        {
                            tel = Browser.FindElements(By.TagName("table"))[1];
                            tel.FindElement(By.ClassName("btn-a")).Click();

                            if (timing != 0) Thread.Sleep(timing);
                        }
                        catch (NoSuchElementException) { done = true; }
                    }
                }
                else
                {
                    while (!done)
                    {
                        try
                        {
                            tdone = false;
                            tel = Browser.FindElements(By.TagName("table"))[1];
                            tlim = tel.FindElements(By.TagName("tr")).Count;
                            for (i = 0; i < tlim && tdone == false; i++) {
                                if (whiteL.Contains(tel.FindElements(By.TagName("tr"))[i].FindElement(By.TagName("td")).FindElement(By.TagName("a")).Text))
                                {
                                    tel.FindElements(By.TagName("tr"))[i].FindElement(By.ClassName("btn-a")).Click();
                                    if (timing != 0) Thread.Sleep(timing);
                                    tdone = true;
                                }
                            }
                            if (!tdone) done = true;
                        }
                        catch (NoSuchElementException) {done = true; }
                    }
                }
            }
            if (grayItem != 0)
            {
                Browser.Navigate().GoToUrl(url + "/user/rack");
                if (timing != 0) Thread.Sleep(timing);
                done = false;

                while (!done)
                {
                    try
                    {
                        lim = Browser.FindElement(By.XPath("//div[@class='content-inner']//table//tbody")).FindElements(By.TagName("tr")).Count;
                    }
                    catch (NoSuchElementException) { done = true; }
                    if (!done)
                    {
                        done = true;
                        for (i = 0; i < lim && done == true; i++)
                        {
                            telem = Browser.FindElement(By.XPath("//div[@class='content-inner']//table//tbody")).FindElements(By.TagName("tr"))[i];
                            tempstr = telem.FindElement(By.ClassName("font14")).FindElement(By.TagName("a")).Text;
                            if (specialList.Contains(tempstr) == false)
                            {
                                try
                                {
                                    if (grayItem == 1)
                                    {
                                        fine = true;
                                        if (fine)
                                        {
                                            done = false;
                                            tlim = telem.FindElements(By.ClassName("btn-equip")).Count;
                                            tdone = false;
                                            for (k = 0; k < tlim && tdone == false; k++)
                                            {
                                                try
                                                {
                                                    telem.FindElement(By.PartialLinkText("Сбыть")).Click();
                                                    if (timing != 0) Thread.Sleep(timing);
                                                    Browser.FindElement(By.XPath("//a[@class='btn-a blue mt5']")).Click();
                                                    if (timing != 0) Thread.Sleep(timing);
                                                    tdone = true;
                                                }
                                                catch (NoSuchElementException) { }
                                            }
                                        }
                                    }
                                }
                                catch (NoSuchElementException)
                                {
                                    done = true;
                                }
                            }
                        }
                        if (i != lim) done = false;
                        else done = true;
                    }
                }
            }
        }

        //fine
        private void DoRobbery()
        {
            bool check = true;
            IWebElement elem;
            //statusLabel.Text = "Статус: патрули/грабежи";
            Browser.Navigate().GoToUrl(url + "/robbery");
            try
            {
                elem = Browser.FindElement(By.XPath("a[@class='btn-a _robbery mt20']"));
                check = false;
            }
            catch (NoSuchElementException) { }
            if (check)
            {
                while (check)
                {
                    try
                    {
                        elem = Browser.FindElement(By.ClassName("btn-a"));
                        if (elem.GetAttribute("class").Contains("mt20"))
                        {
                            check = false;
                        }
                        else
                        {
                            elem.Click();
                            Thread.Sleep(timing);
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        check = false;
                    }
                    catch (StaleElementReferenceException)
                    {
                        Thread.Sleep(timing);
                        Browser.Navigate().Refresh();
                    }
                }
            }

        }

        //fine
        private void GoDistrict()
        {
            //statusLabel.Text = "Статус: смена района";
            String tmp = taskList[0];
            tmp = tmp.Replace("#district: ", "");
            Browser.Navigate().GoToUrl(url + "/skirmish/enter");
            if (timing != 0) Thread.Sleep(timing);
            if (Browser.FindElements(By.ClassName("subtitle"))[0].Text != tmp)
            {
                Browser.Navigate().GoToUrl(url + "/area/a3");
                if (timing != 0) Thread.Sleep(timing);
                Browser.FindElement(By.PartialLinkText("Посетить")).Click();
                if (timing != 0) Thread.Sleep(timing);
            }
        }

        //fine
        private void DoTasks()
        {
            //statusLabel.Text = "Статус: наводки";
            if (taskList[0].Contains("everyday"))
            {
                Browser.Navigate().GoToUrl(url + "/everyday");
                if (timing != 0) Thread.Sleep(timing);
                try
                {
                    Browser.FindElement(By.ClassName("btn-a")).Click();
                    if (timing != 0) Thread.Sleep(timing);
                }
                catch (NoSuchElementException) { }
            }
            else
            {
                String tmp = taskList[0].Replace("#tasks: ", ""), tval;
                bool fine = true, tf = false;
                IWebElement elem;
                int ti = 0, lim = 0, maxval = 0, maxpos = 0;
                if (tmp.Contains(',')) //любые комплексные задания
                {
                    Browser.Navigate().GoToUrl(url + "/tasks");
                    if (timing != 0) Thread.Sleep(timing);
                    if (!tmp.Contains(Browser.FindElement(By.ClassName("npc-name")).FindElement(By.TagName("span")).Text))
                    {
                        if (Browser.FindElements(By.ClassName("list-btns")).Count > 1) ti = 1;
                        lim = Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat")).Count;
                        for (int i = 0; i < lim; i++)
                        {
                            if (tmp.Contains(Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat"))[i].FindElement(By.TagName("span")).Text))
                            {
                                Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat"))[i].Click();
                                fine = true;
                                break;
                            }
                            else
                            {
                                fine = false;
                            }
                        }
                        if (timing != 0) Thread.Sleep(timing);
                    }

                    if (fine)
                    {
                        try
                        {
                            Browser.FindElements(By.ClassName("list-btns"))[0].FindElement(By.ClassName("btn-flat")).Click();
                            if (timing != 0) Thread.Sleep(timing);
                        }
                        catch (NoSuchElementException) { }

                        lim = Browser.FindElements(By.ClassName("b-content"))[0].FindElements(By.ClassName("btn-a")).Count;
                        for (int i = 0; i < lim && tf == false; i++)
                        {
                            tval = Browser.FindElements(By.ClassName("btn-a"))[i].FindElement(By.TagName("span")).Text;
                            if (tmp.Contains(tval))
                            {
                                Browser.FindElements(By.ClassName("btn-a"))[i].Click();
                                if (timing != 0) Thread.Sleep(timing);
                                Browser.FindElement(By.ClassName("btn-green")).Click();
                                if (timing != 0) Thread.Sleep(timing);
                                try
                                {
                                    elem = Browser.FindElement(By.ClassName("b-title"));
                                    if (elem.Text == "Задание выполнено!")
                                    {
                                        tf = true;
                                    }
                                }
                                catch (NoSuchElementException) { }
                                if (!tf)
                                {
                                    try
                                    {
                                        if (Browser.FindElement(By.ClassName("content-inner")).FindElements(By.ClassName("btn-a")).Count == 2)
                                        {
                                            tf = true;
                                        }
                                    }
                                    catch (NoSuchElementException) { }
                                }
                                if (!tf)
                                {
                                    try
                                    {
                                        elem = Browser.FindElement(By.XPath("//span[@class='feedbackPanelERROR']"));
                                        if (elem.Text == "Ты не можешь сейчас проходить эту заказуху.")
                                        {
                                            tf = true;
                                            Browser.Navigate().GoToUrl(url + "/home");
                                            if (timing != 0) Thread.Sleep(timing);
                                            Browser.FindElement(By.ClassName("b-notice")).FindElement(By.ClassName("b-notice-close")).Click();
                                        }
                                    }
                                    catch (NoSuchElementException) { }
                                }
                                if (!tf)
                                {
                                    Fight(); tf = true;
                                }
                            }
                        }
                    }

                }
                else //Джереми и Тони "Фикса"
                {
                    Browser.Navigate().GoToUrl(url + "/tasks");
                    if (timing != 0) Thread.Sleep(timing);
                    if (Browser.FindElement(By.ClassName("npc-name")).FindElement(By.TagName("span")).Text != tmp)
                    {
                        if (Browser.FindElements(By.ClassName("list-btns")).Count > 1) ti = 1;
                        lim = Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat")).Count;
                        for (int i = 0; i < lim; i++)
                        {
                            if (Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat"))[i].FindElement(By.TagName("span")).Text == tmp)
                            {
                                Browser.FindElements(By.ClassName("list-btns"))[ti].FindElements(By.ClassName("btn-flat"))[i].Click();
                                break;
                            }
                        }
                        if (timing != 0) Thread.Sleep(timing);
                    }
                    try
                    {
                        try
                        {
                            Browser.FindElement(By.ClassName("mt5")).FindElement(By.TagName("a")).Click();
                            if (timing != 0) Thread.Sleep(timing);
                        }
                        catch (NoSuchElementException) { }
                        lim = Browser.FindElement(By.ClassName("b-content")).FindElements(By.ClassName("btn-a")).Count;
                        for (int i = 0; i < lim; i++)
                        {
                            tval = Browser.FindElement(By.ClassName("b-content")).FindElements(By.ClassName("btn-a"))[i].FindElements(By.TagName("span"))[1].Text;
                            tval = tval.Replace("Прогресс: ", "");
                            tval = tval.Remove(tval.IndexOf('/'), tval.Length - tval.IndexOf('/'));
                            if (int.Parse(tval) > maxval)
                            {
                                maxval = int.Parse(tval); maxpos = i;
                            }
                        }
                        Browser.FindElements(By.ClassName("btn-a"))[maxpos].Click();
                        if (timing != 0) Thread.Sleep(timing);
                        while (fine)
                        {
                            try
                            {
                                Browser.FindElement(By.ClassName("btn-green")).Click();
                                if (timing != 0) Thread.Sleep(timing);
                                Browser.FindElements(By.ClassName("btn-a"))[0].Click();
                                if (timing != 0) Thread.Sleep(timing);
                            }
                            catch (NoSuchElementException) { fine = false; }
                        }
                    }
                    catch (NoSuchElementException) { fine = true; }

                }
            }
        }

        //fine
        private void DoMissions()
        {
            IWebElement elem;
            bool done = false;
            string tmp = taskList[0].Replace("#missions: ", "");
            byte t;
            //statusLabel.Text = "Статус: выбор заказухи";
            Browser.Navigate().GoToUrl(url + "/party");
            if (timing != 0) Thread.Sleep(timing);
            try
            {
                elem = Browser.FindElement(By.ClassName("b-title")).FindElement(By.TagName("span"));
                //если команда уже есть - выход
                if (elem.Text == "Команда")
                {
                    Browser.FindElement(By.PartialLinkText("Покинуть команду")).Click();
                    Thread.Sleep(timing);
                    Browser.FindElement(By.XPath("//a[@class='btn-a blue mt5']")).Click();
                    Thread.Sleep(timing);
                    Browser.Navigate().GoToUrl(url + "/party");
                    Thread.Sleep(timing);
                    elem = Browser.FindElement(By.ClassName("b-title")).FindElement(By.TagName("span"));
                }
                //если команда не создана
                if (elem.Text == "Создание команды")
                {
                    //если не выбраны заказухи
                    if (Browser.FindElement(By.ClassName("filter-el-active")).Text != "Заказухи")
                    {
                        Browser.FindElements(By.ClassName("filter-el"))[0].Click();
                        Thread.Sleep(timing);
                    }
                    t = byte.Parse(tmp);
                    SelectElement sl = new SelectElement(Browser.FindElement(By.Name("partyName")));
                    sl.SelectByText(missions[t]);
                    if (timing != 0) Thread.Sleep(timing);
                    sl = new SelectElement(Browser.FindElement(By.Name("difficulty")));
                    sl.SelectByValue((2).ToString());
                    if (timing != 0) Thread.Sleep(timing);
                    Browser.FindElement(By.Name("partyHidden")).Click();
                    if (timing != 0) Thread.Sleep(timing);
                    Browser.FindElement(By.XPath("//input[@class='btn-a form-submit no-m font-normal']")).Click();
                    if (timing != 0) Thread.Sleep(timing);
                    try
                    {   //привязь или низкий уровень               
                        elem = Browser.FindElement(By.XPath("//div[@class='list-btns mt5']//a[@class='btn-flat']"));
                    }
                    catch (NoSuchElementException)
                    {
                        done = true;
                    }
                    if (!done)
                    {   //удачно создана команда
                        elem.Click();
                        if (timing != 0) Thread.Sleep(timing);
                        else Thread.Sleep(300);
                        Fight();
                        if (timing != 0) Thread.Sleep(timing);
                        else Thread.Sleep(300);
                        CheckReward();
                        if (timing != 0) Thread.Sleep(timing);
                        else Thread.Sleep(300);
                        done = true;
                    }
                }
            }
            catch (NoSuchElementException) { }
        }

        //fine, требует оптимизации
        private void Fight()
        {
            bool done = false, action = false, abst, locked;
            IWebElement elem, telem, aelem;
            int holder;
            //statusLabel.Text = "Статус: бой";
            while (!done)
            {
                done = false;

                //проверка, есть ли бой
                try
                {
                    elem = Browser.FindElement(By.ClassName("b-title"));
                    if (elem.FindElement(By.TagName("span")).Text == "Задание выполнено!" || elem.Text == "Заказуха выполнена!") done = true;
                }
                catch (NoSuchElementException) { }
                if (!done)
                {
                    action = false;
                    try //боевые случаи
                    {
                        elem = Browser.FindElement(By.XPath("//table[@class='mt5']/tbody/tr"));
                        //ремонт
                        if (!action && repair)
                        {
                            try
                            {
                                elem = Browser.FindElement(By.XPath("//a[@class='btn-a btn-repair50 no-m']"));
                                elem.Click();
                                if (timing != 0) Thread.Sleep(timing);
                                action = true;
                            }
                            catch (NoSuchElementException) { }
                        }
                        //способки
                        if (!action)
                        {
                            try
                            {
                                elem = Browser.FindElement(By.XPath("//table[@class='mt5']/tbody/tr"));
                                abst = false;
                                for (int i = 0; i < 4 && action == false && abst == false; i++)
                                {
                                    aelem = elem.FindElements(By.TagName("td"))[i];
                                    locked = aelem.FindElement(By.TagName("a")).GetAttribute("class").Split(' ').Contains("btn-lock");
                                    //точный выстрел
                                    if (aelem.FindElement(By.TagName("img")).GetAttribute("src").Contains("headshot"))
                                    {
                                        if (!locked)
                                        {
                                            try //обёрнут т.к. в кнопке может не быть ссылки
                                            {
                                                telem = Browser.FindElement(By.XPath("//td[@class='enemy-hp-amount']"));
                                                holder = int.Parse(telem.Text);
                                                if (holder >= precCut)
                                                {
                                                    aelem.FindElement(By.TagName("a")).Click();
                                                    if (timing != 0) Thread.Sleep(timing);
                                                    action = true; abst = true;
                                                }
                                            }
                                            catch (NoSuchElementException) { }
                                        }
                                    }
                                    //остальные способки
                                    else if (!abst)
                                    {
                                        if (!locked)
                                        {
                                            try
                                            {
                                                aelem.FindElement(By.TagName("a")).Click();
                                                if (timing != 0) Thread.Sleep(timing);
                                                action = true; abst = true;
                                            }
                                            catch (NoSuchElementException) { }
                                        }
                                    }
                                }
                            }
                            catch (NoSuchElementException) { }
                        }
                        //обычный удар
                        if (!action)
                        {
                            try
                            {
                                Browser.FindElement(By.ClassName("btn-attack")).Click();
                                if (timing != 0) Thread.Sleep(timing);
                            }
                            catch (NoSuchElementException) { }
                        }
                    }
                    catch (NoSuchElementException) //не боевые
                    {
                        //начало/продолжение заказа
                        try
                        {
                            elem = Browser.FindElement(By.XPath("//a[@class='btn-a bright t-c']"));
                            if (elem.Text.Contains("бой")) elem.Click();
                            if (timing != 0) Thread.Sleep(timing);
                            action = true;
                        }
                        catch (NoSuchElementException) { }
                        //завис
                        if (!action)
                        {
                            try
                            {
                                elem = Browser.FindElement(By.XPath("a[@class='btn-a t-c mt5']"));
                                if (elem.Text.Contains("Обновить страницу"))
                                {
                                    elem.Click();
                                    if (timing != 0) Thread.Sleep(timing);
                                    action = true;
                                }
                            }
                            catch (NoSuchElementException) { }
                        }
                        //лечение
                        if (!action)
                        {
                            try
                            {
                                elem = Browser.FindElement(By.ClassName("i-back"));
                                Browser.FindElement(By.ClassName("t-c")).Click();
                                if (timing != 0) Thread.Sleep(timing);
                                Browser.FindElements(By.ClassName("btn-a"))[0].Click();
                                if (timing != 0) Thread.Sleep(timing);
                                Browser.FindElements(By.ClassName("btn-a"))[0].Click();
                                if (timing != 0) Thread.Sleep(timing);
                                action = true;
                            }
                            catch (NoSuchElementException) { }
                        }
                        //ваша команда...
                        if (!action)
                        {
                            try
                            {
                                elem = Browser.FindElement(By.ClassName("feedbackPanelERROR"));
                                Browser.Navigate().Back();
                            }
                            catch (NoSuchElementException) { Browser.Navigate().Back(); }
                        }
                    }
                }
            }
        }

        //fine
        private void CheckReward()
        {
            IWebElement elem;
            //statusLabel.Text = "Статус: награда";
            try
            {
                elem = Browser.FindElement(By.XPath("//div[@class='b-panel-task']//td[2]"));
                if (elem.Text.Contains("выполнено"))
                {
                    elem.FindElement(By.XPath("//a[@class='btn-a btn-team btn-task']")).Click();
                    if (timing != 0) Thread.Sleep(timing);
                    Browser.FindElement(By.XPath("a[@class='btn-a t-c mb5']")).Click();
                    if (timing != 0) Thread.Sleep(timing);
                }
            }
            catch (NoSuchElementException) { }
        }

        //fine
        private void GetOptions()
        {
            String temp;

            if (repairCheck.Checked) repair = true;
            else repair = false;

            if (robberyCheck.Checked) robbery = true;
            else robbery = false;

            if (tasksCheck.Checked) tasks = true;
            else tasks = false;

            grayItem = (byte)graySelect.SelectedIndex;
            specialList = specialBox.Text;
            specialItem = (byte)specialSelect.SelectedIndex;

            specialList = specialBox.Text;

            int.TryParse(precBox.Text, out precCut);
            if (precCut == -1) precCut = 2000;

            timing = timingBar.Value;
        }

        //fine
        private void SetTasks()
        {
            //statusLabel.Text = "Статус: составление маршрута";
            if (robbery)
            {
                taskList.Add("#robbery:");
            }

            if (tasks)
            {
                taskList.Add("#district: Порт Луи");
                taskList.Add("#tasks: everyday");
                if (side)
                {
                    taskList.Add("#tasks: Джереми");
                    /*taskList.Add("#tasks: Зам. начальника, Всем по заслугам");
                    taskList.Add("#tasks: Зам. начальника, Футбол - Порт Луи");
                    taskList.Add("#tasks: Зам. начальника, Футбол - Площадь культуры.");
                    taskList.Add("#tasks: Зам. начальника, Футбол - Старый город");
                    taskList.Add("#tasks: Саня \"Эксперт\", Большие проблемы");
                    taskList.Add("#tasks: Саня \"Эксперт\", Сезон охоты");
                    taskList.Add("#tasks: Саня \"Эксперт\", Черная метка");*/
                }
                else
                {
                    taskList.Add("#tasks: Тони \"Фикса\"");
                    /* taskList.Add("#tasks: Пахан, Всем по заслугам");
                    taskList.Add("#tasks: Пахан, Футбол - Порт Луи");
                    taskList.Add("#tasks: Пахан, Футбол - Площадь культуры.");
                    taskList.Add("#tasks: Пахан, Футбол - Старый город");
                    taskList.Add("#tasks: Саня \"Эксперт\", Большие проблемы");
                    taskList.Add("#tasks: Саня \"Эксперт\", Сезон охоты");
                    taskList.Add("#tasks: Саня \"Эксперт\", Черная метка");*/
                }
            }
            taskList.Add("#missions: " + 0);
            taskList.Add("#missions: " + 1);
            taskList.Add("#missions: " + 2);
            taskList.Add("#missions: " + 3);
            taskList.Add("#items");
            if(bank) taskList.Add("#bank");
        }

        //test
        private void Bank()
        {
            Browser.Navigate().GoToUrl(url + "/home");
            Thread.Sleep(timing);
            Browser.Navigate().GoToUrl(url + "/robbery/bank");
            Thread.Sleep(timing);
            Browser.FindElement(By.XPath("//body/div[1]/div[3]/div[3]/form[1]/input[1]")).Click();
        }

        //test
        private void LogOut()
        {
            Browser.Navigate().GoToUrl(url + "/home");
            Thread.Sleep(timing);
            try
            {
                Browser.FindElement(By.XPath("//a[contains(text(),'выход')]")).Click();
                Thread.Sleep(timing);
                Browser.FindElement(By.XPath("//a[contains(text(),'выход')]")).Click();
                Thread.Sleep(timing);
            }
            catch (NoSuchElementException) { }
            finishTime = DateTime.Now;
            runtime = finishTime - startTime;
            using (StreamWriter sw = File.AppendText(filename)) {
                if (logins[selector] != "") sw.WriteLine(logins[selector]+" за "+runtime.Minutes+":"+runtime.Seconds+" в "+finishTime.Hour+":"+finishTime.Minute);
            }
            selector++;
            if(selector >= logins.Count) selector = 0;
            taskList.Clear();
            init = false;
        }
    }
}
