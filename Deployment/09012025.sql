
-- Password Reset Request
IF NOT EXISTS (
    SELECT 1 
    FROM Master_EmailTemplates 


    WHERE [Identifier] = 'MEDI_1_1_E1'
)
BEGIN
    INSERT INTO Master_EmailTemplates
    (
        [Identifier], 
        [Subject], 
        [PlainTextBody], 
        [HTMLBody], 
		[IsActive],
		[CreatedDate],
        [CreatedBy]
    ) 
    VALUES 
    ( 
        'MEDI_1_1_E1',
        '{{DOMAIN NAME}} : Password Reset Request', 
        'Hello {{USER FIRST NAME}},\n\nForgot your Password?\n\nClick the link below to reset your password:\n\n{{RESET PASSWORD LINK}}{{RESET CODE}}\n\nThis link will expire in 1 hour.\n\nFor your security, do not share this link with anyone.\n\nBest regards,\n{{DOMAIN NAME}} Team', 
        '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" dir="ltr" xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <style type="text/css">
      @import url("https://fonts.googleapis.com/css2?family=Lato:wght@400;700");
    </style>

    <title>Password Reset</title>

    <style type="text/css">
      .dark-mode .bg-fffffe {
        background-color: #fffffe !important;
      }
      .dark-mode .color-000001 {
        color: #000001 !important;
      }

      @media (prefers-color-scheme: dark) {
        html:not(.light-mode) .bg-fffffe {
          background-color: #fffffe !important;
        }
        html:not(.light-mode) .color-000001 {
          color: #000001 !important;
        }
      }

      [data-ogsc] .bg-fffffe {
        background-color: #fffffe !important;
      }
      [data-ogsc] .color-000001 {
        color: #000001 !important;
      }
    </style>

    <meta name="color-scheme" content="light dark" />
    <meta name="supported-color-schemes" content="light dark" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="x-apple-disable-message-reformatting" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style type="text/css">
      u + div .kombai-email-compat__list-with-padding-left {
        padding-left: 0.5em !important;
      }
      .reset-button {
        display: inline-block;
        padding: 10px 15px;
        background-color: #007BFF;
        color: white;
        text-decoration: none;
        border-radius: 5px;
      }
    </style>

    <!--[if mso]>
      <style type="text/css">
        v\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        o\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        w\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        .ExternalClass {
          width: 100%;
        }
        table {
          mso-table-lspace: 0pt;
          mso-table-rspace: 0pt;
        }
        img {
          -ms-interpolation-mode: bicubic;
        }
        .ReadMsgBody {
          width: 100%;
        }
        a {
          background: transparent !important;
          background-color: transparent !important;
        }

        li {
          text-align: -webkit-match-parent;
          display: list-item;
          text-indent: -1em;
        }

        ul,
        ol {
          margin-left: 1em !important;
        }

        p {
          text-indent: 0;
        }
      </style>
    <![endif]-->
  </head>
  <body style="margin: 0; padding: 0">
    <div style="font-size: 0px; line-height: 1px; mso-line-height-rule: exactly; display: none; max-width: 0px; max-height: 0px; opacity: 0; overflow: hidden; mso-hide: all"></div>
    <center lang="en" dir="ltr" style="width: 100%; table-layout: fixed; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%">
        <table cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="#f1fbff" width="600.00" style="background-color: #f1fbff; width: 600px; border-spacing: 0; font-family: Lato, Tahoma, sans-serif; min-width: 600px">
        <tr>
          <td valign="top" width="100.00%" style="padding-bottom: 38px; width: 100%; vertical-align: top">
            <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="width: 100%; border-spacing: 0">
              <tr>
                <td style="padding-bottom: 12px">
                  <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" bgcolor="#d6f3ff" style="background-color: #d6f3ff; background: linear-gradient(90deg, rgba(214, 243, 255, 1) 0%, rgba(148, 215, 246, 1) 100%); padding-bottom: 10px; width: 100%; border-spacing: 0">
                    <tr>
                      <td align="left" valign="top" style="padding-left: 39.76px; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" style="margin: 0; border-spacing: 0">
                          <tr>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <!--[if mso]><td valign="top"><table role="presentation" border="0" cellspacing="0" cellpadding="0"><tr><![endif]-->
                            <td valign="top" width="364.07" style="padding-top: 38.87px; padding-left: 8px; width: 364.07px; vertical-align: top">
                              <a href="{{DOMAIN URL}}" target="_blank" style="text-decoration: none;"><img src="{{MEDIASSIST LOGO}}" alt="logo" width="197" style="max-width: initial; width: 136px; display: block" /></a>
                            </td>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <td valign="top" style="padding-left: 8px; vertical-align: top">
                              <img src="{{HEADER IMAGE}}" alt="HeaderImage" width="78" style="max-width: initial; width: 78px; display: block" />
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td style="padding-top: 12px; padding-left: 32px; padding-right: 32px">
                  <table class="bg-fffffe" cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="white" width="536.00" style="border-radius: 8px; background-color: white; width: 536px; border-spacing: 0; margin-top: -0.5px; border-collapse: separate">
                    <tr>
                      <td align="left" valign="top" width="100.00%" style="padding: 31px 30px 41px 30px; width: 100%; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="margin: 0; width: 100%; border-spacing: 0">
                          <tr>
                            <td style="padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">Hello {{USER FIRST NAME}},</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">Forgot your Password?</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 16px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 24px; mso-line-height-alt: 24px">
                                Click the link below to reset your password.
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-bottom: 16px">
                              <table cellpadding="0" cellspacing="0" border="0" role="presentation">
                                <tr>
                                  <td>
                                    <a href="{{RESET PASSWORD LINK}}{{RESET CODE}}" class="reset-button" style="display: inline-block; padding: 10px 15px; background-color: #007BFF; color: white; text-decoration: none; border-radius: 5px;">Reset Password</a>
                                  </td>
                                </tr>
                              </table>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-bottom: 16px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 24px; mso-line-height-alt: 24px">
                                This link will expire in 1 hour.
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-bottom: 16px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 700; color: black; margin: 0; padding: 0; line-height: 24px; mso-line-height-rule: exactly">
                                For your security, do not share this link with anyone.
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span>Best regards,<br /></span><span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">{{DOMAIN NAME}} Team</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td align="left" style="padding-top: 16px; padding-bottom: 8.75px;">
                              <!--[if mso]> <table role="presentation" border="0" cellspacing="0" cellpadding="0" align="left" width="476" style="width:476px; border-top:1px solid #e0e0e0;"> <tr> <td align="left"> <![endif]-->
                              <div width="476" style="border-top: 1px solid #e0e0e0; width: 476px; mso-border-top-alt: none"></div>
                              <!--[if mso]></td></tr></table><![endif]-->
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8.75px">
                              <p class="color-868686" width="100.00%" style="font-size: 16px; font-weight: 400; color: #868686; margin: 0; padding: 0; width: 100%; line-height: 24px; text-align: center; mso-line-height-alt: 24px">© January 1st 2025 {{DOMAIN NAME}} | <a href="{{POLICY PAGE URL}}" target="_blank" style="text-decoration: none;">Privacy Policy</a></p>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </center>
  </body>
</html>',
        1,
		GETDATE(),
		1 -- CreatedBy User ID
    );
END;
GO

-- Insert for RequestDemo
IF NOT EXISTS (
    SELECT 1 
    FROM Master_EmailTemplates 
    WHERE [Identifier] = 'MEDI_2_1_E1'
)
BEGIN
    INSERT INTO Master_EmailTemplates
    (
        [Identifier], 
        [Subject], 
        [PlainTextBody], 
        [HTMLBody], 
		[IsActive],
		[CreatedDate],
        [CreatedBy]
    )
    VALUES 
    (
        'MEDI_2_1_E1', 
        '{{DOMAIN NAME}} : New Demo Request Submission', 
        'Dear {{DOMAIN NAME}} Team,\n\nA new demo request has been submitted by {{USER NAME}}. Please find the details of the scheduled demo below:\n\nDemo Request Details:\nName: {{USER NAME}}\nContact Information:\nEmail: {{USER EMAIL}}\nPhone: {{USER PHONENUMBER}}\n{{REQUIREMENTS}}\nSubmitted: {{REQUEST TIMESTAMP}}\n\nKindly reach out to the user for confirmation and any further preparations required for the demo session.\n\nPlease let me know if you need any further details or assistance.\n\nBest regards,\n{{DOMAIN NAME}} Team', 
        '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" dir="ltr" xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <style type="text/css">
      @import url("https://fonts.googleapis.com/css2?family=Lato:wght@400;700");
    </style>

    <title>Demo Request Notification</title>

    <style type="text/css">
      .dark-mode .bg-fffffe {
        background-color: #fffffe !important;
      }
      .dark-mode .color-000001 {
        color: #000001 !important;
      }

      @media (prefers-color-scheme: dark) {
        html:not(.light-mode) .bg-fffffe {
          background-color: #fffffe !important;
        }
        html:not(.light-mode) .color-000001 {
          color: #000001 !important;
        }
      }

      [data-ogsc] .bg-fffffe {
        background-color: #fffffe !important;
      }
      [data-ogsc] .color-000001 {
        color: #000001 !important;
      }
    </style>

    <meta name="color-scheme" content="light dark" />
    <meta name="supported-color-schemes" content="light dark" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="x-apple-disable-message-reformatting" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style type="text/css">
      u + div .kombai-email-compat__list-with-padding-left {
        padding-left: 0.5em !important;
      }
    </style>

    <!--[if mso]>
      <style type="text/css">
        v\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        o\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        w\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        .ExternalClass {
          width: 100%;
        }
        table {
          mso-table-lspace: 0pt;
          mso-table-rspace: 0pt;
        }
        img {
          -ms-interpolation-mode: bicubic;
        }
        .ReadMsgBody {
          width: 100%;
        }
        a {
          background: transparent !important;
          background-color: transparent !important;
        }

        li {
          text-align: -webkit-match-parent;
          display: list-item;
          text-indent: -1em;
        }

        ul,
        ol {
          margin-left: 1em !important;
        }

        p {
          text-indent: 0;
        }
      </style>
    <![endif]-->
  </head>
  <body style="margin: 0; padding: 0">
    <div style="font-size: 0px; line-height: 1px; mso-line-height-rule: exactly; display: none; max-width: 0px; max-height: 0px; opacity: 0; overflow: hidden; mso-hide: all"></div>
    <center lang="en" dir="ltr" style="width: 100%; table-layout: fixed; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%">
        <table cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="#f1fbff" width="600.00" style="background-color: #f1fbff; width: 600px; border-spacing: 0; font-family: Lato, Tahoma, sans-serif; min-width: 600px">
        <tr>
          <td valign="top" width="100.00%" style="padding-bottom: 38px; width: 100%; vertical-align: top">
            <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="width: 100%; border-spacing: 0">
              <tr>
                <td style="padding-bottom: 12px">
                  <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" bgcolor="#d6f3ff" style="background-color: #d6f3ff; background: linear-gradient(90deg, rgba(214, 243, 255, 1) 0%, rgba(148, 215, 246, 1) 100%); padding-bottom: 10px; width: 100%; border-spacing: 0">
                    <tr>
                      <td align="left" valign="top" style="padding-left: 39.76px; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" style="margin: 0; border-spacing: 0">
                          <tr>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <!--[if mso]><td valign="top"><table role="presentation" border="0" cellspacing="0" cellpadding="0"><tr><![endif]-->
                            <td valign="top" width="364.07" style="padding-top: 38.87px; padding-left: 8px; width: 364.07px; vertical-align: top">
                              <a href="{{DOMAIN URL}}" target="_blank" style="text-decoration: none;"><img src="{{MEDIASSIST LOGO}}" alt="logo" width="197" style="max-width: initial; width: 136px; display: block" /></a>
                            </td>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <td valign="top" style="padding-left: 8px; vertical-align: top">
                              <img src="{{HEADER IMAGE}}" alt="HeaderImage" width="78" style="max-width: initial; width: 78px; display: block" />
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td style="padding-top: 12px; padding-left: 32px; padding-right: 32px">
                  <table class="bg-fffffe" cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="white" width="536.00" style="border-radius: 8px; background-color: white; width: 536px; border-spacing: 0; margin-top: -0.5px; border-collapse: separate">
                    <tr>
                      <td align="left" valign="top" width="100.00%" style="padding: 31px 30px 41px 30px; width: 100%; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="margin: 0; width: 100%; border-spacing: 0">
                          <tr>
                            <td style="padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">Dear {{DOMAIN NAME}} Team,</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">A new demo request has been submitted by <span style="font-size: 16px; font-weight: 700">{{USER NAME}}</span>. Please find the details of the scheduled demo below:</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Demo Request Details:</span><br />
                                <span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Name</span><span>: {{USER NAME}}<br /><span class="color-000001" style="font-size: 16px; font-weight: 700; color: black; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Contact Information</span><span>:<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Email: {{USER EMAIL}}<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Phone: {{USER PHONENUMBER}}<br />{{REQUIREMENTS}}<br /></span><span class="color-000001" style="font-size: 16px; font-weight: 700; color: black; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Submitted</span><span>: {{REQUEST TIMESTAMP}}</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 24px; mso-line-height-alt: 24px">Kindly reach out to the user for confirmation and any further preparations required for the demo session.</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 24px; mso-line-height-alt: 24px">Please let me know if you need any further details or assistance.</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span>Best regards,<br /></span><span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">{{DOMAIN NAME}} Team</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td align="left" style="padding-top: 16px; padding-bottom: 8.75px;">
                              <!--[if mso]> <table role="presentation" border="0" cellspacing="0" cellpadding="0" align="left" width="476" style="width:476px; border-top:1px solid #e0e0e0;"> <tr> <td align="left"> <![endif]-->
                              <div width="476" style="border-top: 1px solid #e0e0e0; width: 476px; mso-border-top-alt: none"></div>
                              <!--[if mso]></td></tr></table><![endif]-->
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8.75px">
                              <p class="color-868686" width="100.00%" style="font-size: 16px; font-weight: 400; color: #868686; margin: 0; padding: 0; width: 100%; line-height: 24px; text-align: center; mso-line-height-alt: 24px">© January 1st 2025 {{DOMAIN NAME}} | <a href="{{POLICY PAGE URL}}" target="_blank" style="text-decoration: none;">Privacy Policy</a></p>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </center>
  </body>
</html>',
        1,
		GETDATE(),
		1 -- CreatedBy User ID
    );
END;

-- Insert for RequestDemo
IF NOT EXISTS (
    SELECT 1 
    FROM Master_EmailTemplates 
    WHERE [Identifier] = 'MEDI_2_1_E2'
)
BEGIN
    INSERT INTO Master_EmailTemplates
    (
        [Identifier], 
        [Subject], 
        [PlainTextBody], 
        [HTMLBody], 
		[IsActive],
		[CreatedDate],
        [CreatedBy]
    )
    VALUES 
    (
        'MEDI_2_1_E2', 
        '{{DOMAIN NAME}} : New Request Submission', 
        'Dear {{DOMAIN NAME}} Team,\n\nA new request has been submitted by {{USER NAME}}. Please find the details below:\n\nRequest Details:\nName: {{USER NAME}}\nContact Information:\nEmail: {{USER EMAIL}}\nPhone: {{USER PHONENUMBER}}\n{{REQUIREMENTS}}\nSubmitted: {{REQUEST TIMESTAMP}}\n\nKindly reach out to the user for further discussion regarding {{DOMAIN NAME}} AI.\n\nPlease let me know if you need any further details or assistance.\n\nBest regards,\n{{DOMAIN NAME}} Team', 
        '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" dir="ltr" xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <style type="text/css">
      @import url("https://fonts.googleapis.com/css2?family=Lato:wght@400;700");
    </style>

    <title>Request Notification</title>

    <style type="text/css">
      .dark-mode .bg-fffffe {
        background-color: #fffffe !important;
      }
      .dark-mode .color-000001 {
        color: #000001 !important;
      }

      @media (prefers-color-scheme: dark) {
        html:not(.light-mode) .bg-fffffe {
          background-color: #fffffe !important;
        }
        html:not(.light-mode) .color-000001 {
          color: #000001 !important;
        }
      }

      [data-ogsc] .bg-fffffe {
        background-color: #fffffe !important;
      }
      [data-ogsc] .color-000001 {
        color: #000001 !important;
      }
    </style>

    <meta name="color-scheme" content="light dark" />
    <meta name="supported-color-schemes" content="light dark" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="x-apple-disable-message-reformatting" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style type="text/css">
      u + div .kombai-email-compat__list-with-padding-left {
        padding-left: 0.5em !important;
      }
    </style>

    <!--[if mso]>
      <style type="text/css">
        v\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        o\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        w\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        .ExternalClass {
          width: 100%;
        }
        table {
          mso-table-lspace: 0pt;
          mso-table-rspace: 0pt;
        }
        img {
          -ms-interpolation-mode: bicubic;
        }
        .ReadMsgBody {
          width: 100%;
        }
        a {
          background: transparent !important;
          background-color: transparent !important;
        }

        li {
          text-align: -webkit-match-parent;
          display: list-item;
          text-indent: -1em;
        }

        ul,
        ol {
          margin-left: 1em !important;
        }

        p {
          text-indent: 0;
        }
      </style>
    <![endif]-->
  </head>
  <body style="margin: 0; padding: 0">
    <div style="font-size: 0px; line-height: 1px; mso-line-height-rule: exactly; display: none; max-width: 0px; max-height: 0px; opacity: 0; overflow: hidden; mso-hide: all"></div>
    <center lang="en" dir="ltr" style="width: 100%; table-layout: fixed; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%">
        <table cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="#f1fbff" width="600.00" style="background-color: #f1fbff; width: 600px; border-spacing: 0; font-family: Lato, Tahoma, sans-serif; min-width: 600px">
        <tr>
          <td valign="top" width="100.00%" style="padding-bottom: 38px; width: 100%; vertical-align: top">
            <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="width: 100%; border-spacing: 0">
              <tr>
                <td style="padding-bottom: 12px">
                  <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" bgcolor="#d6f3ff" style="background-color: #d6f3ff; background: linear-gradient(90deg, rgba(214, 243, 255, 1) 0%, rgba(148, 215, 246, 1) 100%); padding-bottom: 10px; width: 100%; border-spacing: 0">
                    <tr>
                      <td align="left" valign="top" style="padding-left: 39.76px; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" style="margin: 0; border-spacing: 0">
                          <tr>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <!--[if mso]><td valign="top"><table role="presentation" border="0" cellspacing="0" cellpadding="0"><tr><![endif]-->
                            <td valign="top" width="364.07" style="padding-top: 38.87px; padding-left: 8px; width: 364.07px; vertical-align: top">
                              <a href="{{DOMAIN URL}}" target="_blank" style="text-decoration: none;"><img src="{{MEDIASSIST LOGO}}" alt="logo" width="197" style="max-width: initial; width: 136px; display: block" /></a>
                            </td>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <td valign="top" style="padding-left: 8px; vertical-align: top">
                              <img src="{{HEADER IMAGE}}" alt="HeaderImage" width="78" style="max-width: initial; width: 78px; display: block" />
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td style="padding-top: 12px; padding-left: 32px; padding-right: 32px">
                  <table class="bg-fffffe" cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="white" width="536.00" style="border-radius: 8px; background-color: white; width: 536px; border-spacing: 0; margin-top: -0.5px; border-collapse: separate">
                    <tr>
                      <td align="left" valign="top" width="100.00%" style="padding: 31px 30px 41px 30px; width: 100%; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="margin: 0; width: 100%; border-spacing: 0">
                          <tr>
                            <td style="padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">Dear {{DOMAIN NAME}} Team,</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">A request has been submitted by <span style="font-size: 16px; font-weight: 700">{{USER NAME}}</span>. Please find the details below:</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Request Details:</span><br />
                                <span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Name</span><span>: {{USER NAME}}<br /><span class="color-000001" style="font-size: 16px; font-weight: 700; color: black; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Contact Information</span><span>:<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Email: {{USER EMAIL}}<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Phone: {{USER PHONENUMBER}}<br />{{REQUIREMENTS}}<br /></span><span class="color-000001" style="font-size: 16px; font-weight: 700; color: black; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Submitted</span><span>: {{REQUEST TIMESTAMP}}</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 24px; mso-line-height-alt: 24px">Kindly reach out to the user for further discussion regarding {{DOMAIN NAME}} AI.</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 24px; mso-line-height-alt: 24px">Please let me know if you need any further details or assistance.</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span>Best regards,<br /></span><span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">{{DOMAIN NAME}} Team</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td align="left" style="padding-top: 16px; padding-bottom: 8.75px;">
                              <!--[if mso]> <table role="presentation" border="0" cellspacing="0" cellpadding="0" align="left" width="476" style="width:476px; border-top:1px solid #e0e0e0;"> <tr> <td align="left"> <![endif]-->
                              <div width="476" style="border-top: 1px solid #e0e0e0; width: 476px; mso-border-top-alt: none"></div>
                              <!--[if mso]></td></tr></table><![endif]-->
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8.75px">
                              <p class="color-868686" width="100.00%" style="font-size: 16px; font-weight: 400; color: #868686; margin: 0; padding: 0; width: 100%; line-height: 24px; text-align: center; mso-line-height-alt: 24px">© January 1st 2025 {{DOMAIN NAME}} | <a href="{{POLICY PAGE URL}}" target="_blank" style="text-decoration: none;">Privacy Policy</a></p>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </center>
  </body>
</html>',
        1,
		GETDATE(),
		1 -- CreatedBy User ID
    );
END;

-- Insert for ContactUsForSubscriptionForRegisteredUser
IF NOT EXISTS (
    SELECT 1 
    FROM Master_EmailTemplates 
    WHERE [Identifier] = 'MEDI_3_1_E1'
)
BEGIN
    INSERT INTO Master_EmailTemplates
    (
        [Identifier], 
        [Subject], 
        [PlainTextBody], 
        [HTMLBody], 
		[IsActive],
		[CreatedDate],
        [CreatedBy]
    )
    VALUES 
    (
        'MEDI_3_1_E1', 
        '{{DOMAIN NAME}} : Plan Inquiry from Registered User', 
        'Dear {{DOMAIN NAME}} Team,\n\nA registered user has inquired about a {{PLAN NAME}} plan. Below are the details:\n\nName: {{USER NAME}}\nContact Information:\nEmail: {{USER EMAIL}}\nPhone: {{USER PHONENUMBER}}\n{{REQUIREMENTS}}\nSubmitted: {{REQUEST TIMESTAMP}}\n\nPlease follow up with the user promptly.\n\nBest regards,\n{{DOMAIN NAME}} Team', 
        '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" dir="ltr" xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <style type="text/css">
      @import url("https://fonts.googleapis.com/css2?family=Lato:wght@400;700");
    </style>

    <title>ContactUsForSubscriptionForRegisteredUser</title>
    <!--[if (!mso)&(!ie)]>These<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>are<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>for<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>outlook<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>live<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>that<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>removes<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>the first<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>10 well-formed<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>conditional comments<!-- -->
    <!--<![endif]-->
    <!--[if gte mso 9]>
      <xml>
        <o:OfficeDocumentSettings xmlns:o="urn:schemas-microsoft-com:office:office">
          <o:AllowPNG />
          <o:PixelsPerInch>96</o:PixelsPerInch>
        </o:OfficeDocumentSettings>
      </xml>
    <![endif]-->

    <style type="text/css">
      .dark-mode .bg-fffffe {
        background-color: #fffffe !important;
      }
      .dark-mode .color-000001 {
        color: #000001 !important;
      }

      @media (prefers-color-scheme: dark) {
        html:not(.light-mode) .bg-fffffe {
          background-color: #fffffe !important;
        }
        html:not(.light-mode) .color-000001 {
          color: #000001 !important;
        }
      }

      [data-ogsc] .bg-fffffe {
        background-color: #fffffe !important;
      }
      [data-ogsc] .color-000001 {
        color: #000001 !important;
      }
    </style>

    <meta name="color-scheme" content="light dark" />

    <meta name="supported-color-schemes" content="light dark" />

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <!--[if !mso]><!-->
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <!--<![endif]-->

    <meta name="x-apple-disable-message-reformatting" />

    <style></style>

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style type="text/css">
      u + div .kombai-email-compat__list-with-padding-left {
        padding-left: 0.5em !important;
      }
    </style>

    <!--[if mso]>
      <style type="text/css">
        v\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        o\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        w\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        .ExternalClass {
          width: 100%;
        }
        table {
          mso-table-lspace: 0pt;
          mso-table-rspace: 0pt;
        }
        img {
          -ms-interpolation-mode: bicubic;
        }
        .ReadMsgBody {
          width: 100%;
        }
        a {
          background: transparent !important;
          background-color: transparent !important;
        }

        li {
          text-align: -webkit-match-parent;
          display: list-item;
          text-indent: -1em;
        }

        ul,
        ol {
          margin-left: 1em !important;
        }

        p {
          text-indent: 0;
        }
      </style>
    <![endif]-->
  </head>
  <body style="margin: 0; padding: 0">
    <div style="font-size: 0px; line-height: 1px; mso-line-height-rule: exactly; display: none; max-width: 0px; max-height: 0px; opacity: 0; overflow: hidden; mso-hide: all"></div>
    <center lang="en" dir="ltr" style="width: 100%; table-layout: fixed; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%">
        <table cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="#f1fbff" width="600.00" style="background-color: #f1fbff; width: 600px; border-spacing: 0; font-family: Lato, Tahoma, sans-serif; min-width: 600px">
        <tr>
          <td valign="top" width="100.00%" style="padding-bottom: 38px; width: 100%; vertical-align: top">
            <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="width: 100%; border-spacing: 0">
              <tr>
                <td style="padding-bottom: 12px">
                  <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" bgcolor="#d6f3ff" style="background-color: #d6f3ff; background: linear-gradient(90deg, rgba(214, 243, 255, 1) 0%, rgba(148, 215, 246, 1) 100%); padding-bottom: 10px; width: 100%; border-spacing: 0">
                    <tr>
                      <td align="left" valign="top" style="padding-left: 39.76px; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" style="margin: 0; border-spacing: 0">
                          <tr>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <!--[if mso]><td valign="top"><table role="presentation" border="0" cellspacing="0" cellpadding="0"><tr><![endif]-->
                            <td valign="top" width="364.07" style="padding-top: 38.87px; padding-left: 8px; width: 364.07px; vertical-align: top">
                              <a href="{{DOMAIN URL}}" target="_blank" style="text-decoration: none;"><img src="{{MEDIASSIST LOGO}}" alt="logo" width="197" style="max-width: initial; width: 136px; display: block" /></a>
                            </td>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <td valign="top" style="padding-left: 8px; vertical-align: top">
                              <img src="{{HEADER IMAGE}}" alt="HeaderImage" width="78" style="max-width: initial; width: 78px; display: block" />
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td style="padding-top: 12px; padding-left: 32px; padding-right: 32px">
                  <table class="bg-fffffe" cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="white" width="536.00" style="border-radius: 8px; background-color: white; width: 536px; border-spacing: 0; margin-top: -0.5px; border-collapse: separate">
                    <tr>
                      <td align="left" valign="top" width="100.00%" style="padding: 31px 30px 41px 30px; width: 100%; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="margin: 0; width: 100%; border-spacing: 0">
                          <tr>
                            <td style="padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">Dear {{DOMAIN NAME}} Team,</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px"><span>A registered user has inquired about </span><span style="font-size: 16px; font-weight: 700">{{PLAN NAME}}</span><span>. Below are the details:</span></p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Name</span><span>: {{USER NAME}}<br /><span class="color-000001" style="font-size: 16px; font-weight: 700; color: black; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Contact Information</span><span>:<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Email: {{USER EMAIL}}<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Phone: {{USER PHONENUMBER}}<br />{{REQUIREMENTS}}<br /></span><span class="color-000001" style="font-size: 16px; font-weight: 700; color: black; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Submitted</span><span>: {{REQUEST TIMESTAMP}}</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 24px; mso-line-height-alt: 24px">Please follow up with the user promptly.</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span>Best regards,<br /></span><span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">{{DOMAIN NAME}} Team</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td align="left" style="padding-top: 16px; padding-bottom: 8.75px;">
                              <!--[if mso]> <table role="presentation" border="0" cellspacing="0" cellpadding="0" align="left" width="476" style="width:476px; border-top:1px solid #e0e0e0;"> <tr> <td align="left"> <![endif]-->
                              <div width="476" style="border-top: 1px solid #e0e0e0; width: 476px; mso-border-top-alt: none"></div>
                              <!--[if mso]></td></tr></table><![endif]-->
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8.75px">
                              <p class="color-868686" width="100.00%" style="font-size: 16px; font-weight: 400; color: #868686; margin: 0; padding: 0; width: 100%; line-height: 24px; text-align: center; mso-line-height-alt: 24px">© January 1st 2025 {{DOMAIN NAME}} | <a href="{{POLICY PAGE URL}}" target="_blank" style="text-decoration: none;">Privacy Policy</a></p>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </center>
  </body>
</html>
',
        1,
		GETDATE(),
		1 -- CreatedBy User ID
    );
END;

-- Insert for ContactUsForSubscriptionForGuestUser
IF NOT EXISTS (
    SELECT 1 
    FROM Master_EmailTemplates 
    WHERE [Identifier] = 'MEDI_3_1_E2'
)
BEGIN
    INSERT INTO Master_EmailTemplates
    (
        [Identifier], 
        [Subject], 
        [PlainTextBody], 
        [HTMLBody], 
		[IsActive],
		[CreatedDate],
        [CreatedBy]
    )
    VALUES 
    (
        'MEDI_3_1_E2', 
        '{{DOMAIN NAME}} : New Paid Plan Inquiry', 
        'Dear {{DOMAIN NAME}} Team,\n\nA user has inquired about a {{PLAN NAME}} plan. Below are the details:\n\nName: {{USER NAME}}\nContact Information:\nEmail: {{USER EMAIL}}\nPhone: {{USER PHONENUMBER}}\n{{REQUIREMENTS}}\nSubmitted: {{REQUEST TIMESTAMP}}\n\nPlease follow up with the user promptly.\n\nBest regards,\n{{DOMAIN NAME}} Team', 
        '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" dir="ltr" xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <style type="text/css">
      @import url("https://fonts.googleapis.com/css2?family=Lato:wght@400;700");
    </style>

    <title>ContactUsForSubscriptionForGuestUser</title>
    <!--[if (!mso)&(!ie)]>These<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>are<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>for<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>outlook<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>live<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>that<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>removes<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>the first<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>10 well-formed<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>conditional comments<!-- -->
    <!--<![endif]-->
    <!--[if gte mso 9]>
      <xml>
        <o:OfficeDocumentSettings xmlns:o="urn:schemas-microsoft-com:office:office">
          <o:AllowPNG />
          <o:PixelsPerInch>96</o:PixelsPerInch>
        </o:OfficeDocumentSettings>
      </xml>
    <![endif]-->

    <style type="text/css">
      .dark-mode .bg-fffffe {
        background-color: #fffffe !important;
      }
      .dark-mode .color-000001 {
        color: #000001 !important;
      }

      @media (prefers-color-scheme: dark) {
        html:not(.light-mode) .bg-fffffe {
          background-color: #fffffe !important;
        }
        html:not(.light-mode) .color-000001 {
          color: #000001 !important;
        }
      }

      [data-ogsc] .bg-fffffe {
        background-color: #fffffe !important;
      }
      [data-ogsc] .color-000001 {
        color: #000001 !important;
      }
    </style>

    <meta name="color-scheme" content="light dark" />

    <meta name="supported-color-schemes" content="light dark" />

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <!--[if !mso]><!-->
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <!--<![endif]-->

    <meta name="x-apple-disable-message-reformatting" />

    <style></style>

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style type="text/css">
      u + div .kombai-email-compat__list-with-padding-left {
        padding-left: 0.5em !important;
      }
    </style>

    <!--[if mso]>
      <style type="text/css">
        v\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        o\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        w\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        .ExternalClass {
          width: 100%;
        }
        table {
          mso-table-lspace: 0pt;
          mso-table-rspace: 0pt;
        }
        img {
          -ms-interpolation-mode: bicubic;
        }
        .ReadMsgBody {
          width: 100%;
        }
        a {
          background: transparent !important;
          background-color: transparent !important;
        }

        li {
          text-align: -webkit-match-parent;
          display: list-item;
          text-indent: -1em;
        }

        ul,
        ol {
          margin-left: 1em !important;
        }

        p {
          text-indent: 0;
        }
      </style>
    <![endif]-->
  </head>
  <body style="margin: 0; padding: 0">
    <div style="font-size: 0px; line-height: 1px; mso-line-height-rule: exactly; display: none; max-width: 0px; max-height: 0px; opacity: 0; overflow: hidden; mso-hide: all"></div>
    <center lang="en" dir="ltr" style="width: 100%; table-layout: fixed; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%">
        <table cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="#f1fbff" width="600.00" style="background-color: #f1fbff; width: 600px; border-spacing: 0; font-family: Lato, Tahoma, sans-serif; min-width: 600px">
        <tr>
          <td valign="top" width="100.00%" style="padding-bottom: 38px; width: 100%; vertical-align: top">
            <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="width: 100%; border-spacing: 0">
              <tr>
                <td style="padding-bottom: 12px">
                  <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" bgcolor="#d6f3ff" style="background-color: #d6f3ff; background: linear-gradient(90deg, rgba(214, 243, 255, 1) 0%, rgba(148, 215, 246, 1) 100%); padding-bottom: 10px; width: 100%; border-spacing: 0">
                    <tr>
                      <td align="left" valign="top" style="padding-left: 39.76px; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" style="margin: 0; border-spacing: 0">
                          <tr>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <!--[if mso]><td valign="top"><table role="presentation" border="0" cellspacing="0" cellpadding="0"><tr><![endif]-->
                            <td valign="top" width="364.07" style="padding-top: 38.87px; padding-left: 8px; width: 364.07px; vertical-align: top">
                              <a href="{{DOMAIN URL}}" target="_blank" style="text-decoration: none;"><img src="{{MEDIASSIST LOGO}}" alt="logo" width="197" style="max-width: initial; width: 136px; display: block" /></a>
                            </td>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <td valign="top" style="padding-left: 8px; vertical-align: top">
                              <img src="{{HEADER IMAGE}}" alt="HeaderImage" width="78" style="max-width: initial; width: 78px; display: block" />
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td style="padding-top: 12px; padding-left: 32px; padding-right: 32px">
                  <table class="bg-fffffe" cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="white" width="536.00" style="border-radius: 8px; background-color: white; width: 536px; border-spacing: 0; margin-top: -0.5px; border-collapse: separate">
                    <tr>
                      <td align="left" valign="top" width="100.00%" style="padding: 31px 30px 41px 30px; width: 100%; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="margin: 0; width: 100%; border-spacing: 0">
                          <tr>
                            <td style="padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">Dear {{DOMAIN NAME}} Team,</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px"><span>A user has inquired about </span><span style="font-size: 16px; font-weight: 700">{{PLAN NAME}}</span><span>. Below are the details:</span></p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Name</span><span>: {{USER NAME}}<br /><span class="color-000001" style="font-size: 16px; font-weight: 700; color: black; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Contact Information</span><span>:<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Email: {{USER EMAIL}}<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Phone: {{USER PHONENUMBER}}<br />{{REQUIREMENTS}}<br /></span><span class="color-000001" style="font-size: 16px; font-weight: 700; color: black; text-align: left; line-height: 24px; mso-line-height-rule: exactly">Submitted</span><span>: {{REQUEST TIMESTAMP}}</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 24px; mso-line-height-alt: 24px">Please follow up with the user promptly.</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span>Best regards,<br /></span><span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">{{DOMAIN NAME}} Team</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td align="left" style="padding-top: 16px; padding-bottom: 8.75px;">
                              <!--[if mso]> <table role="presentation" border="0" cellspacing="0" cellpadding="0" align="left" width="476" style="width:476px; border-top:1px solid #e0e0e0;"> <tr> <td align="left"> <![endif]-->
                              <div width="476" style="border-top: 1px solid #e0e0e0; width: 476px; mso-border-top-alt: none"></div>
                              <!--[if mso]></td></tr></table><![endif]-->
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8.75px">
                              <p class="color-868686" width="100.00%" style="font-size: 16px; font-weight: 400; color: #868686; margin: 0; padding: 0; width: 100%; line-height: 24px; text-align: center; mso-line-height-alt: 24px">© January 1st 2025 {{DOMAIN NAME}} | <a href="{{POLICY PAGE URL}}" target="_blank" style="text-decoration: none;">Privacy Policy</a></p>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </center>
  </body>
</html>',
        1,
		GETDATE(),
		1 -- CreatedBy User ID
    );
END;

-- Insert for ThankyouMailToUserOnPricingEnquiry
IF NOT EXISTS (
    SELECT 1 
    FROM Master_EmailTemplates 
    WHERE [Identifier] = 'MEDI_3_1_E3'
)
BEGIN
    INSERT INTO Master_EmailTemplates
    (
        [Identifier], 
        [Subject], 
        [PlainTextBody], 
        [HTMLBody], 
		[IsActive],
		[CreatedDate],
        [CreatedBy]
    )
    VALUES 
    (
        'MEDI_3_1_E3', 
        '{{DOMAIN NAME}} : Thank You for Your Inquiry!', 
        'Dear {{USER NAME}},\n\nGreetings from {{DOMAIN NAME}} AI.\n\nThank you for your interest in {{DOMAIN NAME}} AI. Our team will reach out to you shortly to discuss your requirements.\n\nBest regards,\n{{DOMAIN NAME}} Team', 
        '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" dir="ltr" xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <style type="text/css">
      @import url("https://fonts.googleapis.com/css2?family=Lato:wght@400;700");
    </style>

    <title>ThankyouMailToUserOnPricingEnquiry</title>
    <!--[if (!mso)&(!ie)]>These<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>are<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>for<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>outlook<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>live<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>that<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>removes<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>the first<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>10 well-formed<!-- -->
    <!--<![endif]-->
    <!--[if (!mso)&(!ie)]>conditional comments<!-- -->
    <!--<![endif]-->
    <!--[if gte mso 9]>
      <xml>
        <o:OfficeDocumentSettings xmlns:o="urn:schemas-microsoft-com:office:office">
          <o:AllowPNG />
          <o:PixelsPerInch>96</o:PixelsPerInch>
        </o:OfficeDocumentSettings>
      </xml>
    <![endif]-->

    <style type="text/css">
      .dark-mode .bg-fffffe {
        background-color: #fffffe !important;
      }
      .dark-mode .color-000001 {
        color: #000001 !important;
      }

      @media (prefers-color-scheme: dark) {
        html:not(.light-mode) .bg-fffffe {
          background-color: #fffffe !important;
        }
        html:not(.light-mode) .color-000001 {
          color: #000001 !important;
        }
      }

      [data-ogsc] .bg-fffffe {
        background-color: #fffffe !important;
      }
      [data-ogsc] .color-000001 {
        color: #000001 !important;
      }
    </style>

    <meta name="color-scheme" content="light dark" />

    <meta name="supported-color-schemes" content="light dark" />

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <!--[if !mso]><!-->
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <!--<![endif]-->

    <meta name="x-apple-disable-message-reformatting" />

    <style></style>

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style type="text/css">
      u + div .kombai-email-compat__list-with-padding-left {
        padding-left: 0.5em !important;
      }
    </style>

    <!--[if mso]>
      <style type="text/css">
        v\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        o\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        w\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        .ExternalClass {
          width: 100%;
        }
        table {
          mso-table-lspace: 0pt;
          mso-table-rspace: 0pt;
        }
        img {
          -ms-interpolation-mode: bicubic;
        }
        .ReadMsgBody {
          width: 100%;
        }
        a {
          background: transparent !important;
          background-color: transparent !important;
        }

        li {
          text-align: -webkit-match-parent;
          display: list-item;
          text-indent: -1em;
        }

        ul,
        ol {
          margin-left: 1em !important;
        }

        p {
          text-indent: 0;
        }
      </style>
    <![endif]-->
  </head>
  <body style="margin: 0; padding: 0">
    <div style="font-size: 0px; line-height: 1px; mso-line-height-rule: exactly; display: none; max-width: 0px; max-height: 0px; opacity: 0; overflow: hidden; mso-hide: all"></div>
    <center lang="en" dir="ltr" style="width: 100%; table-layout: fixed; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%">
      <table cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="#f1fbff" width="600.00" style="background-color: #f1fbff; width: 600px; border-spacing: 0; font-family: Lato, Tahoma, sans-serif; min-width: 600px">
        <tr>
          <td valign="top" width="100.00%" style="padding-bottom: 38px; width: 100%; vertical-align: top">
            <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="width: 100%; border-spacing: 0">
              <tr>
                <td style="padding-bottom: 12px">
                  <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" bgcolor="#d6f3ff" style="background-color: #d6f3ff; background: linear-gradient(90deg, rgba(214, 243, 255, 1) 0%, rgba(148, 215, 246, 1) 100%); padding-bottom: 10px; width: 100%; border-spacing: 0">
                    <tr>
                      <td align="left" valign="top" style="padding-left: 39.76px; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" style="margin: 0; border-spacing: 0">
                          <tr>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <!--[if mso]><td valign="top"><table role="presentation" border="0" cellspacing="0" cellpadding="0"><tr><![endif]-->
                            <td valign="top" width="364.07" style="padding-top: 38.87px; padding-left: 8px; width: 364.07px; vertical-align: top">
                              <a href="{{DOMAIN URL}}" target="_blank" style="text-decoration: none;"><img src="{{MEDIASSIST LOGO}}" alt="logo" width="197" style="max-width: initial; width: 136px; display: block" /></a>
                            </td>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <td valign="top" style="padding-left: 8px; vertical-align: top">
                              <img src="{{HEADER IMAGE}}" alt="HeaderImage" width="78" style="max-width: initial; width: 78px; display: block" />
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td style="padding-top: 12px; padding-left: 32px; padding-right: 32px">
                  <table class="bg-fffffe" cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="white" width="536.00" style="border-radius: 8px; background-color: white; width: 536px; border-spacing: 0; margin-top: -0.5px; border-collapse: separate">
                    <tr>
                      <td align="left" valign="top" width="100.00%" style="padding: 31px 30px 41px 30px; width: 100%; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="margin: 0; width: 100%; border-spacing: 0">
                          <tr>
                            <td style="padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">Dear {{USER NAME}},</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px"><span>Greetings from {{DOMAIN NAME}} AI. </span></p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 24px; mso-line-height-alt: 24px">Thank you for your interest in {{DOMAIN NAME}} AI. Our team will reach out to you shortly to discuss your requirements..</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span>Best regards,<br /></span><span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">{{DOMAIN NAME}} Team</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td align="left" style="padding-top: 16px; padding-bottom: 8.75px;">
                              <!--[if mso]> <table role="presentation" border="0" cellspacing="0" cellpadding="0" align="left" width="476" style="width:476px; border-top:1px solid #e0e0e0;"> <tr> <td align="left"> <![endif]-->
                              <div width="476" style="border-top: 1px solid #e0e0e0; width: 476px; mso-border-top-alt: none"></div>
                              <!--[if mso]></td></tr></table><![endif]-->
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8.75px">
                              <p class="color-868686" width="100.00%" style="font-size: 16px; font-weight: 400; color: #868686; margin: 0; padding: 0; width: 100%; line-height: 24px; text-align: center; mso-line-height-alt: 24px">© January 1st 2025 {{DOMAIN NAME}} | <a href="{{POLICY PAGE URL}}" target="_blank" style="text-decoration: none;">Privacy Policy</a></p>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </center>
  </body>
</html>',
        1,
		GETDATE(),
		1 -- CreatedBy User ID
    );
END;

-- Insert for ShareReport
IF NOT EXISTS (
    SELECT 1 
    FROM Master_EmailTemplates 
    WHERE [Identifier] = 'MEDI_4_1_E1'
)
BEGIN
    INSERT INTO Master_EmailTemplates
    (
        [Identifier], 
        [Subject], 
        [PlainTextBody], 
        [HTMLBody], 
		[IsActive],
		[CreatedDate],
        [CreatedBy]
    )
    VALUES 
    (
        'MEDI_4_1_E1', 
        '{{HOSPITALNAME}} : {{REPORTNAME}}', 
        'Dear {{RECIPIENTNAME}},\n\n We hope this message finds you well.\n\nThe consultation with {{DOCTORNAME}} took place on {{CONSULTATIONDATE}}. The {{REPORTNAME}} has been prepared and shared with you by {{HOSPITALNAME}}.\n\nPlease find the attached report for your reference. If you have any questions or need further assistance, do not hesitate to reach out.\n\nBest regards,\n{{HOSPITALNAME}}\n{{HOSPITALADDRESS}}', 
        '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en" dir="ltr" xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <style type="text/css">
      @import url("https://fonts.googleapis.com/css2?family=Lato:wght@400;700");
    </style>

    <title>Share report</title>

    <style type="text/css">
      .dark-mode .bg-fffffe {
        background-color: #fffffe !important;
      }
      .dark-mode .color-000001 {
        color: #000001 !important;
      }

      @media (prefers-color-scheme: dark) {
        html:not(.light-mode) .bg-fffffe {
          background-color: #fffffe !important;
        }
        html:not(.light-mode) .color-000001 {
          color: #000001 !important;
        }
      }

      [data-ogsc] .bg-fffffe {
        background-color: #fffffe !important;
      }
      [data-ogsc] .color-000001 {
        color: #000001 !important;
      }
    </style>

    <meta name="color-scheme" content="light dark" />
    <meta name="supported-color-schemes" content="light dark" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="x-apple-disable-message-reformatting" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <style type="text/css">
      u + div .kombai-email-compat__list-with-padding-left {
        padding-left: 0.5em !important;
      }
    </style>

    <!--[if mso]>
      <style type="text/css">
        v\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        o\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        w\:* {
          behavior: url(#default#VML);
          display: inline-block;
        }
        .ExternalClass {
          width: 100%;
        }
        table {
          mso-table-lspace: 0pt;
          mso-table-rspace: 0pt;
        }
        img {
          -ms-interpolation-mode: bicubic;
        }
        .ReadMsgBody {
          width: 100%;
        }
        a {
          background: transparent !important;
          background-color: transparent !important;
        }

        li {
          text-align: -webkit-match-parent;
          display: list-item;
          text-indent: -1em;
        }

        ul,
        ol {
          margin-left: 1em !important;
        }

        p {
          text-indent: 0;
        }
      </style>
    <![endif]-->
  </head>
  <body style="margin: 0; padding: 0">
    <div style="font-size: 0px; line-height: 1px; mso-line-height-rule: exactly; display: none; max-width: 0px; max-height: 0px; opacity: 0; overflow: hidden; mso-hide: all"></div>
    <center lang="en" dir="ltr" style="width: 100%; table-layout: fixed; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%">
         <table cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="#f1fbff" width="600.00" style="background-color: #f1fbff; width: 600px; border-spacing: 0; font-family: Lato, Tahoma, sans-serif; min-width: 600px">
        <tr>
          <td valign="top" width="100.00%" style="padding-bottom: 38px; width: 100%; vertical-align: top">
            <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="width: 100%; border-spacing: 0">
              <tr>
                <td style="padding-bottom: 12px">
                  <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" bgcolor="#d6f3ff" style="background-color: #d6f3ff; background: linear-gradient(90deg, rgba(214, 243, 255, 1) 0%, rgba(148, 215, 246, 1) 100%); padding-bottom: 10px; width: 100%; border-spacing: 0">
                    <tr>
                      <td align="left" valign="top" style="padding-left: 39.76px; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" style="margin: 0; border-spacing: 0">
                          <tr>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <!--[if mso]><td valign="top"><table role="presentation" border="0" cellspacing="0" cellpadding="0"><tr><![endif]-->
                            <td valign="top" width="364.07" style="padding-top: 38.87px; padding-left: 8px; width: 364.07px; vertical-align: top">
                              <a href="{{DOMAIN URL}}" target="_blank" style="text-decoration: none;"><img src="{{MEDIASSIST LOGO}}" alt="logo" width="197" style="max-width: initial; width: 136px; display: block" /></a>
                            </td>
                            <!--[if mso]></tr></table></td><![endif]-->
                            <td valign="top" style="padding-left: 8px; vertical-align: top">
                              <img src="{{HEADER IMAGE}}" alt="HeaderImage" width="78" style="max-width: initial; width: 78px; display: block" />
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td style="padding-top: 12px; padding-left: 32px; padding-right: 32px">
                  <table class="bg-fffffe" cellpadding="0" cellspacing="0" border="0" role="presentation" bgcolor="white" width="536.00" style="border-radius: 8px; background-color: white; width: 536px; border-spacing: 0; margin-top: -0.5px; border-collapse: separate">
                    <tr>
                      <td align="left" valign="top" width="100.00%" style="padding: 31px 30px 41px 30px; width: 100%; vertical-align: top">
                        <table cellpadding="0" cellspacing="0" border="0" role="presentation" width="100.00%" style="margin: 0; width: 100%; border-spacing: 0">
                          <tr>
                            <td style="padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">Dear {{RECIPIENTNAME}},</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">We hope this message finds you well.</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 19px; mso-line-height-alt: 20px">The consultation with {{DOCTORNAME}} took place on {{CONSULTATIONDATE}}. The {{REPORTNAME}} has been prepared and shared with you by {{HOSPITALNAME}}.</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px; padding-bottom: 8px">
                              <p class="color-000001" width="100.00%" style="font-size: 16px; font-weight: 400; color: black; margin: 0; padding: 0; width: 100%; line-height: 24px; mso-line-height-alt: 24px">Please find the attached report for your reference. If you have any questions or need further assistance, do not hesitate to reach out.</p>
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8px">
                              <p class="color-000001" style="font-size: 16px; font-weight: 400; text-align: left; line-height: 24px; color: black; mso-line-height-rule: exactly; margin: 0; padding: 0">
                                <span>Best regards,<br /></span><span style="font-size: 16px; font-weight: 700; text-align: left; line-height: 24px; mso-line-height-rule: exactly">{{HOSPITALNAME}} <br /> {{HOSPITALADDRESS}}</span>
                              </p>
                            </td>
                          </tr>
                          <tr>
                            <td align="left" style="padding-top: 16px; padding-bottom: 8.75px;">
                              <!--[if mso]> <table role="presentation" border="0" cellspacing="0" cellpadding="0" align="left" width="476" style="width:476px; border-top:1px solid #e0e0e0;"> <tr> <td align="left"> <![endif]-->
                              <div width="476" style="border-top: 1px solid #e0e0e0; width: 476px; mso-border-top-alt: none"></div>
                              <!--[if mso]></td></tr></table><![endif]-->
                            </td>
                          </tr>
                          <tr>
                            <td style="padding-top: 8.75px">
                              <p class="color-868686" width="100.00%" style="font-size: 16px; font-weight: 400; color: #868686; margin: 0; padding: 0; width: 100%; line-height: 24px; text-align: center; mso-line-height-alt: 24px">© January 1st 2025 {{DOMAIN NAME}} | <a href="{{POLICY PAGE URL}}" target="_blank" style="text-decoration: none;">Privacy Policy</a></p>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </center>
  </body>
</html>',
        1,
		GETDATE(),
		1 -- CreatedBy User ID
    );
END;
GO