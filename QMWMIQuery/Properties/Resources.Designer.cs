﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuickMon.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("QuickMon.Properties.Resources", typeof(Resources).Assembly);
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
        
        internal static System.Drawing.Bitmap doc_refresh {
            get {
                object obj = ResourceManager.GetObject("doc_refresh", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;config&gt;
        ///  &lt;wmi namespace=&quot;root\CIMV2&quot; machines=&quot;.&quot;&gt;
        ///    &lt;stateQuery syntax=&quot;SELECT FreeSpace FROM Win32_LogicalDisk where Caption = &apos;C:&apos;&quot; returnValueIsInt=&quot;True&quot; returnValueInverted=&quot;True&quot; 
        ///                warningValue=&quot;1048576000&quot; errorValue=&quot;524288000&quot; successValue=&quot;[any]&quot;
        ///                useRowCountAsValue=&quot;False&quot; /&gt;
        ///    &lt;detailQuery syntax=&quot;SELECT Caption, Size, FreeSpace, Description FROM Win32_LogicalDisk where Caption = &apos;C:&apos;&quot; columnNames=&quot;Caption, Size, FreeSpace, Description&quot; keyColumn=&quot;0&quot; /&gt;
        /// </summary>
        internal static string WMIQueryEmptyConfig {
            get {
                return ResourceManager.GetString("WMIQueryEmptyConfig", resourceCulture);
            }
        }
    }
}