using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Luval.UiPath.NER.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using edu.stanford.nlp.ie.crf;
using System.IO;

namespace Luval.UiPath.NER.Activities
{
    [LocalizedDisplayName(nameof(Resources.GetNamedEntities_DisplayName))]
    [LocalizedDescription(nameof(Resources.GetNamedEntities_Description))]
    public class GetNamedEntities : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.GetNamedEntities_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetNamedEntities_Text_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Text { get; set; }

        [LocalizedDisplayName(nameof(Resources.GetNamedEntities_Entities_DisplayName))]
        [LocalizedDescription(nameof(Resources.GetNamedEntities_Entities_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<DataTable> Entities { get; set; }

        #endregion


        #region Constructors

        public GetNamedEntities()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Text == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Text)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {

            // Inputs
            var text = Text.Get(context);

            var classifier = Helper.GetClassifier();
            var xml = classifier.classifyToString(text, "xml", true);
            var result = OutputParser.Parse(xml);

            // Outputs
            return (ctx) => {
                Entities.Set(ctx, result);
            };
        }

        #endregion
    }
}

