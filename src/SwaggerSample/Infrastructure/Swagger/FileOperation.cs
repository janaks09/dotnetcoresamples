using Swashbuckle.SwaggerGen.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.Swagger.Model;

namespace SwaggerSample.Infrastructure.Swagger
{
    public class FileOperation : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.OperationId.ToLower() == "apifileuploadpost")
            {
                operation.Parameters.Clear();//Clearing parameters
                operation.Consumes.Add("application/form-data");
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "File",
                    In = "formData",
                    Description = "Uplaod Image",
                    Required = true,
                    Type = "file"
                });
            }
        }
    }
}
