using Core.Utilities.MailHelper;
using Entities.ShareModels;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Consumer
{
    public class ReceiveEmailConsumer : IConsumer<SendEmailCommand>
    {
        private readonly IEmailHelper _mailHelper;

        public ReceiveEmailConsumer(IEmailHelper mailHelper)
        {
            _mailHelper = mailHelper;
        }

        public async  Task Consume(ConsumeContext<SendEmailCommand> context)
        {
            _mailHelper.SendEmail(context.Message.Email, context.Message.Token, true);
        }
    }
}
