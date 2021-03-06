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
    internal class UIFileUpload {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UIFileUpload() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CLAIM.Ressources.FormTexts.UIFileUpload", typeof(UIFileUpload).Assembly);
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
        ///   Looks up a localized string similar to Le fichier ne répond pas aux spécifications de la politique de sécurité..
        /// </summary>
        internal static string AVBlockFile {
            get {
                return ResourceManager.GetString("AVBlockFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Le fichier n’a pas pu être téléchargé en raison d’un virus. Veuillez supprimer et vérifier le fichier..
        /// </summary>
        internal static string AVFileWithMalware {
            get {
                return ResourceManager.GetString("AVFileWithMalware", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Le format du fichier n’est pas autorisé..
        /// </summary>
        internal static string ERExtension {
            get {
                return ResourceManager.GetString("ERExtension", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Veuillez joindre un fichier valide..
        /// </summary>
        internal static string ERFichierAbsent {
            get {
                return ResourceManager.GetString("ERFichierAbsent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Veuillez supprimer les fichiers en erreur pour poursuivre..
        /// </summary>
        internal static string ERGenerale {
            get {
                return ResourceManager.GetString("ERGenerale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Une erreur est survenue lors du téléversement du fichier. Veuillez communiquer avec le Support technique pour résoudre ce problème..
        /// </summary>
        internal static string ERInternalError {
            get {
                return ResourceManager.GetString("ERInternalError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Le nom du fichier excède la longueur maximale de {0} caractères..
        /// </summary>
        internal static string ERLongueurNomFichierMaximale {
            get {
                return ResourceManager.GetString("ERLongueurNomFichierMaximale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Le nom du fichier est vide..
        /// </summary>
        internal static string ERLongueurNomFichierMinimale {
            get {
                return ResourceManager.GetString("ERLongueurNomFichierMinimale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La limite de téléversement est fixée à {0} fichiers simultanément..
        /// </summary>
        internal static string ERMaxNbFichier {
            get {
                return ResourceManager.GetString("ERMaxNbFichier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Le fichier dépasse la taille maximale..
        /// </summary>
        internal static string ERTailleMaximale {
            get {
                return ResourceManager.GetString("ERTailleMaximale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Le fichier n&apos;atteint pas la taille minimale..
        /// </summary>
        internal static string ERTailleMinimale {
            get {
                return ResourceManager.GetString("ERTailleMinimale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to tif, tiff, pdf, jpeg, jpg, png, gif, doc, docx, xls, xlsx.
        /// </summary>
        internal static string ExtensionsAutorisees {
            get {
                return ResourceManager.GetString("ExtensionsAutorisees", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Aucun fichier joint.
        /// </summary>
        internal static string NoFileAttached {
            get {
                return ResourceManager.GetString("NoFileAttached", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Formats acceptés :.
        /// </summary>
        internal static string TitreExtensionsAutorisees {
            get {
                return ResourceManager.GetString("TitreExtensionsAutorisees", resourceCulture);
            }
        }
    }
}
