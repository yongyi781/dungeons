﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dungeons.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.1.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\yongy\\OneDrive\\Pictures\\RS Screenshots\\DG Maps")]
        public string MapSaveLocation {
            get {
                return ((string)(this["MapSaveLocation"]));
            }
            set {
                this["MapSaveLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2703, 370")]
        public global::System.Drawing.Point MapLocation {
            get {
                return ((global::System.Drawing.Point)(this["MapLocation"]));
            }
            set {
                this["MapLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\yongy\\OneDrive\\Pictures\\Winterfaces")]
        public string WinterfaceSaveLocation {
            get {
                return ((string)(this["WinterfaceSaveLocation"]));
            }
            set {
                this["WinterfaceSaveLocation"] = value;
            }
        }
    }
}
