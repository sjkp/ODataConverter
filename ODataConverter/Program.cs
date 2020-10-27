using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Vocabularies;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.OData;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ODataConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            IEdmModel model = GetEdmModel();
            var myModel = new MyClass(model);
            OpenApiConvertSettings settings = new OpenApiConvertSettings
            {
                // configuration
                
            };
            OpenApiDocument document = myModel.ConvertToOpenApi(settings);
            for (int i=document.Paths.Count-1;i>1;i--)
            {
                document.Paths.Remove(document.Paths.ElementAt(i).Key);
            }

            for(int i=document.Components.Schemas.Count-1;i>=0;i--)
            {
                var elm = document.Components.Schemas.ElementAt(i);
                if (elm.Key == "Microsoft.Dynamics.CRM.account")
                {
                    continue;
                }
                document.Components.Schemas.Remove(elm.Key);
            }

            document.Components.Examples.Clear();


            using (var fs = System.IO.File.Open("openapi2.json", System.IO.FileMode.Create))
            {
                document.SerializeAsJson(fs, OpenApiSpecVersion.OpenApi3_0);
            }
            //var outputYAML = document.SerializeAsYaml( OpenApiSpecVersion.OpenApi3_0);
            
            //System.IO.File.WriteAllText("openapi.json", outputJSON);
            //System.IO.File.WriteAllText("openapi.yaml", outputYAML);
            Console.WriteLine("Hello World!");
        }

        public static IEdmModel GetEdmModel()
        {
            string csdlFilePath = @"c:\projects\odata-openapi\ODataV4Metadata.xml";
            string csdl = System.IO.File.ReadAllText(csdlFilePath);
            IEdmModel model = CsdlReader.Parse(XElement.Parse(csdl).CreateReader());
            
            return model;
        }
    }

    public class MyEdmEntityContainer : IEdmEntityContainer
    {
        private readonly IEdmEntityContainer edmEntityContainer;

        public MyEdmEntityContainer(IEdmEntityContainer edmEntityContainer)
        {
            this.edmEntityContainer = edmEntityContainer;
        }
        public IEnumerable<IEdmEntityContainerElement> Elements => edmEntityContainer.Elements.Where(s => s.Name == "accounts");

        public EdmSchemaElementKind SchemaElementKind => this.edmEntityContainer.SchemaElementKind;

        public string Namespace => this.edmEntityContainer.Namespace;

        public string Name => this.edmEntityContainer.Name;

        public IEdmEntitySet FindEntitySet(string setName)
        {
            return this.edmEntityContainer.FindEntitySet(setName);
        }

        public IEnumerable<IEdmOperationImport> FindOperationImports(string operationName)
        {
            return this.edmEntityContainer.FindOperationImports(operationName);
        }

        public IEdmSingleton FindSingleton(string singletonName)
        {
            return this.edmEntityContainer.FindSingleton(singletonName);
        }
    }

    public class MyClass : IEdmModel
    {
        private readonly IEdmModel edmModel;

        public MyClass(IEdmModel edmModel)
        {
            this.edmModel = edmModel;
        }
        public IEnumerable<IEdmSchemaElement> SchemaElements => this.edmModel.SchemaElements;

        public IEnumerable<IEdmVocabularyAnnotation> VocabularyAnnotations => this.edmModel.VocabularyAnnotations;

        public IEnumerable<IEdmModel> ReferencedModels => this.edmModel.ReferencedModels;

        public IEnumerable<string> DeclaredNamespaces => this.edmModel.DeclaredNamespaces;

        public IEdmDirectValueAnnotationsManager DirectValueAnnotationsManager => this.edmModel.DirectValueAnnotationsManager;

        public IEdmEntityContainer EntityContainer => new MyEdmEntityContainer(this.edmModel.EntityContainer);

        public IEnumerable<IEdmOperation> FindDeclaredBoundOperations(IEdmType bindingType)
        {
            return this.edmModel.FindDeclaredBoundOperations(bindingType);
        }

        public IEnumerable<IEdmOperation> FindDeclaredBoundOperations(string qualifiedName, IEdmType bindingType)
        {
            return this.edmModel.FindDeclaredBoundOperations(qualifiedName, bindingType);
        }

        public IEnumerable<IEdmOperation> FindDeclaredOperations(string qualifiedName)
        {
            return this.edmModel.FindDeclaredOperations(qualifiedName);
        }

        public IEdmTerm FindDeclaredTerm(string qualifiedName)
        {
            return this.edmModel.FindDeclaredTerm(qualifiedName);
        }

        public IEdmSchemaType FindDeclaredType(string qualifiedName)
        {
            return this.edmModel.FindDeclaredType(qualifiedName);
        }

        public IEnumerable<IEdmVocabularyAnnotation> FindDeclaredVocabularyAnnotations(IEdmVocabularyAnnotatable element)
        {
            return this.edmModel.FindDeclaredVocabularyAnnotations(element);
        }

        public IEnumerable<IEdmStructuredType> FindDirectlyDerivedTypes(IEdmStructuredType baseType)
        {
            return this.edmModel.FindDirectlyDerivedTypes(baseType);
        }
    }

}
