using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;

namespace lab31v19
{

    public interface ISearchEngine
    {
        IEnumerable<string> Search(string query);
        bool IsAvailable();
    }

    public interface IIndexer
    {
        void LogSearch(string query, int resultsCount);
        void IndexNewDocument(string content);
    }

    public class SearchService
    {
        private readonly ISearchEngine _engine;
        private readonly IIndexer _indexer;

        public SearchService(ISearchEngine engine, IIndexer indexer)
        {
            _engine = engine;
            _indexer = indexer;
        }

        public List<string> ExecuteSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query cannot be empty");

            if (!_engine.IsAvailable())
                return new List<string>();

            var results = _engine.Search(query).ToList();
            _indexer.LogSearch(query, results.Count);

            return results;
        }

        public bool AddDocument(string? content) // Додано ?, щоб прибрати попередження про null
        {
            if (string.IsNullOrEmpty(content) || content.Length < 5) 
                return false;

            _indexer.IndexNewDocument(content);
            return true;
        }
    }

    public class SearchServiceTests
    {
        private readonly Mock<ISearchEngine> _engineMock;
        private readonly Mock<IIndexer> _indexerMock;
        private readonly SearchService _service;

        public SearchServiceTests()
        {
            _engineMock = new Mock<ISearchEngine>();
            _indexerMock = new Mock<IIndexer>();
            _service = new SearchService(_engineMock.Object, _indexerMock.Object);
        }

        [Fact]
        public void Test1_Search_ReturnsCorrectResults()
        {
            _engineMock.Setup(e => e.IsAvailable()).Returns(true);
            _engineMock.Setup(e => e.Search("it")).Returns(new List<string> { "Lab", "Moq" });

            var result = _service.ExecuteSearch("it");

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Test2_Search_ThrowsExceptionOnEmptyQuery()
        {
            Assert.Throws<ArgumentException>(() => _service.ExecuteSearch(""));
        }

        [Fact]
        public void Test3_Search_ReturnsEmptyWhenEngineOffline()
        {
            _engineMock.Setup(e => e.IsAvailable()).Returns(false);

            var result = _service.ExecuteSearch("anything");

            Assert.Empty(result);
            _engineMock.Verify(e => e.Search(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Test4_Search_CallsLogSearchOnce()
        {
            _engineMock.Setup(e => e.IsAvailable()).Returns(true);
            _engineMock.Setup(e => e.Search("test")).Returns(new List<string> { "res" });

            _service.ExecuteSearch("test");

            _indexerMock.Verify(i => i.LogSearch("test", 1), Times.Once);
        }

        [Fact]
        public void Test5_AddDocument_CallsIndexerWhenValid()
        {
            string doc = "Valid content";
            
            var result = _service.AddDocument(doc);

            Assert.True(result);
            _indexerMock.Verify(i => i.IndexNewDocument(doc), Times.Once);
        }

        [Fact]
        public void Test6_AddDocument_ReturnsFalseWhenShort()
        {
            var result = _service.AddDocument("123");

            Assert.False(result);
            _indexerMock.Verify(i => i.IndexNewDocument(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Test7_Search_VerifiesAvailabilityIsChecked()
        {
            _engineMock.Setup(e => e.IsAvailable()).Returns(true);
            _engineMock.Setup(e => e.Search(It.IsAny<string>())).Returns(new List<string>());

            _service.ExecuteSearch("query");

            _engineMock.Verify(e => e.IsAvailable(), Times.AtLeastOnce);
        }

        [Fact]
        public void Test8_AddDocument_HandlesNullContent()
        {
            var result = _service.AddDocument(null);

            Assert.False(result);
            _indexerMock.Verify(i => i.IndexNewDocument(It.IsAny<string>()), Times.Never);
        }
    }
}