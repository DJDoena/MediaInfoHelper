﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.8.3928.0.
// 
namespace DoenaSoft.MediaInfoHelper.DataObjects.FFProbeMetaXml {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ffprobe", Namespace="", IsNullable=false)]
    public partial class FFProbeMeta {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("stream", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public Stream[] streams;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Format format;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FileName;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Stream {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Disposition disposition;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tag", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Tag[] tag;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string index;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codec_name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codec_long_name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string profile;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codec_type;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codec_time_base;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codec_tag_string;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codec_tag;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort width;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool widthSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort height;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool heightSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort coded_width;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool coded_widthSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort coded_height;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool coded_heightSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string has_b_frames;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sample_aspect_ratio;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string display_aspect_ratio;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pix_fmt;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string level;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string chroma_location;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string field_order;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string refs;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string is_avc;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nal_length_size;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string r_frame_rate;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string avg_frame_rate;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string time_base;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string start_pts;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string start_time;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bits_per_raw_sample;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sample_fmt;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort sample_rate;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sample_rateSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte channels;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool channelsSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string channel_layout;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bits_per_sample;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dmix_mode;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ltrt_cmixlev;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ltrt_surmixlev;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string loro_cmixlev;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string loro_surmixlev;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bit_rate;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string duration_ts;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string duration;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Disposition {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @default;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dub;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string original;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string comment;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lyrics;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string karaoke;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string forced;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string hearing_impaired;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string visual_impaired;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string clean_effects;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string attached_pic;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string timed_thumbnails;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Format {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tag", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Tag[] tag;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string filename;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nb_streams;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nb_programs;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string format_name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string format_long_name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string start_time;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string duration;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string size;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bit_rate;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string probe_score;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Tag {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value;
    }
}
