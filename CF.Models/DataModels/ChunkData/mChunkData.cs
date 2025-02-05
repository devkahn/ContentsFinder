using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CF.Models.DataModels.ChunkData
{
    public class mChunkData : mBase
    {
        [JsonProperty("doc_type")]
        [Description("문서 종류")]
        public string DocType { get; set; } = string.Empty;

        [JsonProperty("doc_level1")]
        [Description("문서 레벨1")]
        public string DocLevel1 { get; set; } = string.Empty;

        [JsonProperty("doc_level2")]
        [Description("문서 레벨2")]
        public string DocLevel2 { get; set; } = string.Empty;

        [JsonProperty("doc_level3")]
        [Description("문서 레벨3")]
        public string DocLevel3 { get; set; } = string.Empty;

        [JsonProperty("doc_level4")]
        [Description("문서 레벨1")]
        public string DocLevel4 { get; set;} = string.Empty;

        [JsonProperty("doc_level5")]
        [Description("문서 레벨2")]
        public string DocLevel5 { get; set;} = string.Empty;

        [JsonProperty("doc_level6")]
        [Description("문서 레벨3")]
        public string DocLevel6 { get; set; } = string.Empty;

        [JsonProperty("manual_name")]
        [Description("매뉴얼 이름")]
        public string ManualName { get; set; } = string.Empty;

        [JsonProperty("chunk_keyword")]
        [Description("대표 키워드")]
        public string MainKeyword { get; set; } = string.Empty;

        [JsonProperty("chunk_title")]
        [Description("문서 제목")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("chunk_embedding")]
        [Description("문서내용_임베딩용")]
        public string EmbeddingContents { get; set; } = string.Empty;

        [JsonProperty("chunk_retrieve")]
        [Description("문서내용_검색용")]
        public string RetrieveContents { get; set; } = string.Empty;

        [JsonProperty("chunk_generation")]
        [Description("문서내용_답변생성용")]
        public string GenerationContents { get; set; } = string.Empty;

        [JsonProperty("chunk_ref")]
        [Description("문서내용_참고문서")]
        public string ReferenceContents { get; set; } = string.Empty;

        [JsonProperty("image_link")]
        [Description("이미지 링크")]
        public List<string[]> ImageList { get; set; } = new List<string[]>();

        [JsonProperty("image_Items")]
        [Description("이미지 링크_모델링")]
        public List<mChunkImage> ImageLinks { get; set; } = new List<mChunkImage>();
        

        [JsonProperty("large_embed_vec")]
        [Description("벡터")]
        public List<double> Vertors { get; set; } = new List<double>();
    }
}
