﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CLAIM.Ressources.FormTexts {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class UIAdvisor {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UIAdvisor() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CLAIM.Ressources.FormTexts.UIAdvisor", typeof(UIAdvisor).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Veuillez inscrire le code du conseiller (6 caractères)..
        /// </summary>
        internal static string Error_MissingAdvisorCode {
            get {
                return ResourceManager.GetString("Error_MissingAdvisorCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Veuillez inscrire le nom du conseiller..
        /// </summary>
        internal static string Error_MissingAdvisorName {
            get {
                return ResourceManager.GetString("Error_MissingAdvisorName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Veuillez inscrire le numéro de l’agence (3 caractères)..
        /// </summary>
        internal static string Error_MissingAgencyCode {
            get {
                return ResourceManager.GetString("Error_MissingAgencyCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Veuillez inscrire le nom de l’agence..
        /// </summary>
        internal static string Error_MissingAgencyName {
            get {
                return ResourceManager.GetString("Error_MissingAgencyName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Conseiller.
        /// </summary>
        internal static string Header_Advisor {
            get {
                return ResourceManager.GetString("Header_Advisor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Code de conseiller (6 caractères).
        /// </summary>
        internal static string Text_AdvisorCode {
            get {
                return ResourceManager.GetString("Text_AdvisorCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nom du conseiller.
        /// </summary>
        internal static string Text_AdvisorName {
            get {
                return ResourceManager.GetString("Text_AdvisorName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Numéro de l’agence (3 caractères).
        /// </summary>
        internal static string Text_AgencyCode {
            get {
                return ResourceManager.GetString("Text_AgencyCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nom de l’agence.
        /// </summary>
        internal static string Text_AgencyName {
            get {
                return ResourceManager.GetString("Text_AgencyName", resourceCulture);
            }
        }
    }
}
