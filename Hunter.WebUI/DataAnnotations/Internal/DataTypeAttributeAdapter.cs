// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace Hunter.WebUI.DataAnnotations.Internal
{
    public class DataTypeAttributeAdapter : AttributeAdapterBase<DataTypeAttribute>
    {
        public DataTypeAttributeAdapter(DataTypeAttribute attribute, string ruleName, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {
            if (string.IsNullOrEmpty(ruleName))
            {
                //throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, nameof(ruleName));
                throw new ArgumentNullException(nameof(ruleName));
            }

            RuleName = ruleName;
        }

        public string RuleName { get; }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //MergeAttribute(context.Attributes, "data-val", "true");
            //MergeAttribute(context.Attributes, RuleName, GetErrorMessage(context));
            MergeAttribute(context.Attributes, RuleName, GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-msg-" + RuleName, GetErrorMessage(context));
        }

        /// <inheritdoc/>
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.GetDataTypeName());
        }
    }
}
