using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace phidelis_api
{
    public class SwaggerFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId == "POST_PUT" || operation.OperationId == "POST_PUT")
            {
                operation.RequestBody = new OpenApiRequestBody()
                {
                    Content = new Dictionary<string, OpenApiMediaType> {
                {"application/json",
                    new OpenApiMediaType()
                    {

                        Schema = new OpenApiSchema(){
                            Example = new OpenApiObject
                                {
                                    ["name"] = new OpenApiString("string"),
                                    ["docNumber"] = new OpenApiString("string")
                                }
                        }
                    }
                }
            }
                };
            }
            else
            {
                return;
            }

        }
    }
}
