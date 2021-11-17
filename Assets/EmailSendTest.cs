using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using UnityEngine;

public class EmailSendTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            string smtpAddress = "smtp.gmail.com";//"smtp.office365.com";// 
            int portNumber = 587;
            bool enableSSL = true;
            string emailFromAddress = "shilad2020@gmail.com";//"ShilaRani.Das@georgebrown.ca"; //Sender Email Address  
            string password = "*****"; //Sender Password 
            string emailToAddress = "Fernando.Restituto@georgebrown.ca";//"shiladas2007@gmail.com";//// //Receiver Email Address  
            string subject = "Hello";
            string body = "Hello, This is Email sending test using gmail.";
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(emailToAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                    Debug.Log("Send");
                }
            }
        }
    }
}
