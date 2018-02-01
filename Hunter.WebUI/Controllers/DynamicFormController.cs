﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Hunter.WebUI.Controllers
{
    public class DynamicFormController : Controller
    {
        public DynamicFormController(IHostingEnvironment hostingEnvironment, Managers.Manager manager)
        {
            this.HostingEnvironment = hostingEnvironment;
            this.Manager = manager;
        }

        protected IHostingEnvironment HostingEnvironment { get; set; }

        protected Managers.Manager Manager { get; set; }

        public IActionResult List(string id)
        {
            var entity = this.Manager.FormManager.Find(id);
            return View(entity);
        }

        public IActionResult Query(string id, [FromBody]Models.PageParam<Models.DynamicForm.Condition> pageParam)
        {
            var result = this.Manager.DynamicFormManager.Query(id, pageParam);
            return this.Json(result);
        }

        [HttpGet]
        public IActionResult Edit(string id, string dataID)
        {
            var entity = this.Manager.DynamicFormManager.Find(id, dataID);
            if (entity == null)
            {
                entity = new Entities.DynamicForm() { ID = this.Manager.GenerateMongoID };
                var form = this.Manager.FormManager.Find(id);
                form.CopyTo(entity);
                entity.CurrentNode = entity.Nodes.GetStartNode();
            }
            this.ViewData["id"] = id;
            this.ViewData["DataID"] = entity.ID;
            return this.View(entity);
        }

        [HttpPost]
        public IActionResult SaveData(string id, string dataID, [FromBody]Dictionary<string, object> dictionary)
        {
            this.Manager.DynamicFormManager.SaveData(id, dataID, dictionary);
            return this.Ok();
        }

        public IActionResult Next(string id, string dataID, string lineID)
        {
            var result = this.Manager.DynamicFormManager.Next(id, dataID, lineID);
            if (result.Success)
                return this.Ok(result);
            return this.BadRequest(result);
        }

        public IActionResult Finish(string formID, string dataID)
        {
            var result = this.Manager.DynamicFormManager.Finish(formID, dataID);
            if (result.Success)
                return this.Ok(result);
            return this.BadRequest(result);
        }

        public IActionResult Find(string id, string dataID)
        {
            var data = this.Manager.DynamicFormManager.Find(id, dataID);
            return this.Json(data);
        }

        public IActionResult Remove(string id, string dataID)
        {
            this.Manager.DynamicFormManager.Remove(id, dataID);
            return this.Ok();
        }
        
        public IActionResult Download(string id, string dataID, string type)
        {
            var entity = this.Manager.DynamicFormManager.Find(id, dataID);
            var html = this.Manager.DynamicFormManager.GetCompleteHtml(entity.Html, entity.Data, this.HostingEnvironment.WebRootPath);
            if (String.Equals("pdf", type, StringComparison.OrdinalIgnoreCase))
            {
                var stream = new System.IO.MemoryStream();
                var fontProvider = new iText.Html2pdf.Resolver.Font.DefaultFontProvider(true, true, true);
                var converterProperties = new iText.Html2pdf.ConverterProperties();
                converterProperties.SetFontProvider(fontProvider);
                converterProperties.SetCreateAcroForm(true);
                iText.Html2pdf.HtmlConverter.ConvertToPdf(html, stream, converterProperties);
                byte[] bytes = stream.ToArray();
                return this.File(bytes, "application/pdf");
            }
            return this.Content(html, "text/html");
        }

    }
}