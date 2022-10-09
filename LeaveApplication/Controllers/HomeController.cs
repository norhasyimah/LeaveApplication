using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using LeaveApplication.Models;

namespace LeaveApplication.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      var entities = new LeaveApplicationEntities1();


      return View(entities.Applications.ToList());
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    public ActionResult LeaveApplicationList()
    {
      var entities = new LeaveApplicationEntities1();


      return View(entities.Applications.ToList());


    }

    public ActionResult Create()
    {

      return View();

    }
    [HttpPost]
    public ActionResult Create(Application obj)

    {
      if (ModelState.IsValid)
      {
        LeaveApplicationEntities1 db = new LeaveApplicationEntities1();
        db.Applications.Add(obj);
        db.SaveChanges();

        try { 
          MailMessage mail = new MailMessage();
          mail.To.Add(obj.ManagerEmail);
          mail.From = new MailAddress(obj.Email);
          mail.Subject = "Leave Application Request";
          string Body = "Leave Application Request for " + obj.Name;
          mail.Body = Body;
          mail.IsBodyHtml = true;
          SmtpClient smtp = new SmtpClient();
          smtp.Host = "smtp.gmail.com";
          smtp.Port = 587;
          smtp.UseDefaultCredentials = false;
          smtp.Credentials = new System.Net.NetworkCredential("username", "password"); // Enter senders User name and password  
          smtp.EnableSsl = true;
          smtp.Send(mail);
        }
        catch (Exception ex)
        {
          return RedirectToAction("Index");
        }


      }
      return RedirectToAction("Index");
    }


    public ActionResult Update(int id)
    {
      using (var context = new LeaveApplicationEntities1())
      {
        var data = context.Applications.Where(x => x.Id == id).SingleOrDefault();
        return View(data);
      }
    }

    // To specify that this will be
    // invoked when post method is called
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Update(int id, Application model)
    {
      using (var context = new LeaveApplicationEntities1())
      {

        // Use of lambda expression to access
        // particular record from a database
        var data = context.Applications.FirstOrDefault(x => x.Id == id);

        // Checking if any such record exist
        if (data != null)
        {
          data.Name = model.Name;
          data.StartDate = model.StartDate;
          data.EndDate = model.EndDate;
          data.Justification = model.Justification;
          data.ManagerName = model.ManagerName;
          context.SaveChanges();

          // It will redirect to
          // the Read method
          return RedirectToAction("Index");
        }
        else
          return View();
      }
    }


    public ActionResult Delete(int id)
    {
      using (var context = new LeaveApplicationEntities1())
      {
        var data = context.Applications.Where(x => x.Id == id).SingleOrDefault();
        return View(data);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, Application model)
    {
      using (var context = new LeaveApplicationEntities1())
      {
        var data = context.Applications.FirstOrDefault(x => x.Id == id);
        if (data != null)
        {
          context.Applications.Remove(data);
          context.SaveChanges();
          return RedirectToAction("Index");
        }
        else
          return View();
      }
    }

    // GET: /SendMailer/   
    public ActionResult Approve(int id)
    {
      using (var context = new LeaveApplicationEntities1())
      {
        var data = context.Applications.Where(x => x.Id == id).SingleOrDefault();
        return View(data);
      }
    }
    [HttpPost]
    public ActionResult Approve(int id, Application model)
    {
      using (var context = new LeaveApplicationEntities1())
      {
        var data = context.Applications.FirstOrDefault(x => x.Id == id);
        try { 
            MailMessage mail = new MailMessage();
            mail.To.Add(data.Email);
            mail.From = new MailAddress(data.ManagerEmail);
            mail.Subject = "Leave Application has been approved";
            string Body = "Leave Application for "+data.Name+" has been approved";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("username", "password"); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
          return RedirectToAction("Index");
        }
      
        
      }
    }

  }
}
