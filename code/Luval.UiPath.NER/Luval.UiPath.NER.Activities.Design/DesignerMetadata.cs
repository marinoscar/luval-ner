using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using Luval.UiPath.NER.Activities.Design.Designers;
using Luval.UiPath.NER.Activities.Design.Properties;

namespace Luval.UiPath.NER.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(GetNamedEntities), categoryAttribute);
            builder.AddCustomAttributes(typeof(GetNamedEntities), new DesignerAttribute(typeof(GetNamedEntitiesDesigner)));
            builder.AddCustomAttributes(typeof(GetNamedEntities), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
