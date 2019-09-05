using fa2Server;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SignCheck()
        {
            string data = "Y+03DYkELesfGnKu45BqcA==";
            string decryptdata = dataHashFilter.Response_ios460_getSign(data, "874e8dd7-d333-46dd-9a45-f0e5841887c7");
            Assert.AreEqual(decryptdata, "167bbfc5f38504473c28b687a457f272");

            data = "amjDIqdpq1fN0J021QQCi7rcBc6jCgKdOdiSrLe+8id3t98NyRj/Uc0FMMAUcB/p5TJFwUD217b1DwgTC5x2JwcEYQVe7LcjhDqfKvYwNQNvRD3pu/TED3Cuqn5t68okcuS1mzgr5n1nfUuRo4kjsITI0R2I4Npy2p3lbuj9yt7sxmmLH962zBMOi3/tsD8/UwnSY9IY7Fszk3AscbJGNOsd8neiCNXXispyLpDa0B6wxz/oUPrBVf8//TB97fSCoOYNRT/vRxPsyl+lGEdLrIgjas7K+Iefym7ZTt6f7AVFW0xgdvW1nOR1j9K8I8tAUXpeFS8gHh5U2eMN+Kg8pBAKzGwj8R0p4DBOe4pLajwSxlXP196fTTnebygfjptMlN2qnBT1nYgmLabPW+8lSlbrYLtO22BlIMGpHF9I3/t71bS8kbYoQlKmItiuepUFfcqKq1/IKQKK37WVvhzdx4u6CwaynfH7eCf6wA6/QOUP0v5qVHyHCc5+P1+hG25GD2uLh+BcjOmFU6qrS1+eX74dek+EcKKPxPFSbuetXu1wPlvEhp/GyloOegLPfuAmJUw26M5j3+FPSWMI7nAFvpWNVmmT8wR9Abqt2Qzt4ZfoIoX82jBp+HMey+GinldmH0Kbg4UldbI4gB6pYgj5BNYRYcrikDcJ7YWfdjdczkyZFGWLb7Yg4VvxmaiWjhscAmDz+ViFlLrbB9l/80xk/7a8/NaZtSvTJqhGb6xWZ9/s4ZTinHRlemJ8zwXg66556xw6ZTT1jqF04UeX1iXO7jFPFilH/laBA78Wd6JFujs=";
            decryptdata = dataHashFilter.Response_ios460_getSign(data, "874e8dd7-d333-46dd-9a45-f0e5841887c7");
            Assert.AreEqual(decryptdata, "725a12ca36a7f174dc43bd1c88b1881e");

            data = "rpVJwENhonXCAQ94UeLnEWvpMpKnOW/OOcKksazbm/oEie6EEjLDBH/3pAWKeU/GbX/94EeWhbROfwMynlDOh1rwd9ktH5cjDRq1pkb6U789PWBQykURSn5fQFa7JlGz+1xCFZwdhv7ix0iOK7FChj4QrnWBEnCtSAWZUvDvPhJa1YSSO9GKFv0aOzSTv9TTYT6LWeTwyPsbftH/3N/eTAyfw2aj3xLSNtF6K7y7wC/sc3s2dTWiduIBuBn9nyrul7DOVdxlzOfkzxe8+cBIyEVV6TVDs7WghiObIuIb3ks=";
            decryptdata = dataHashFilter.ios460_getSign(data, "d1507484-2b13-49d3-8f6d-ae22ee90152c");
            Assert.AreEqual(decryptdata, "3f020b2e687d1b9e29e9549363fba229");


            Assert.Pass();
        }

        [Test]
        public void Test1()
        {
            string data = "Y7KJKr5vIfkb54hsnrDS5wb+3kWv0TDaGjLMBosxyxcU/ZrQEtuhLOQad9pLntBOP5Q0OouzeenrHGlUvpMCJYe0fj91IJc0l8AqrcSWcPAimZ8T5RpSpkJNxCYu81MpHqxJ0bkbBRCteZFL/BPoltROEctNd3Vnel8+ED07pijgYiEMDp29klr1UYLtUSczDRDfBDdAZv+vQLf0fOWNOensZubAkXH6GVLqtQd4UfFpvcqBCsqETbyOdlK5L9/BlpdeSw95dGm0YNKvoDDfZNqrE+zuIt7t4ZkN0Nrr2DBXzyf/uNGUMqjFX2yLBE8J";
            string decryptdata = dataHashFilter.AESDecrypt(data,"07c6ddc6-d841-47c3-adbe-3b4040f5c37d", "874e8dd7-d333-46dd-9a45-f0e5841887c7", 1567374618);
            string Encryptdata = dataHashFilter.AESEncrypt(decryptdata, "", "",1567235435);
            Assert.AreEqual(data, Encryptdata);
            Assert.Pass();
        }
        [Test]
        public void Test2()
        {
            string data = "tFIUpl2fkj6EfCqIlR3WkQ==";
            //string decryptdata = dataHashFilter.AESDecrypt(data,"07c6ddc6-d841-47c3-adbe-3b4040f5c37d", "874e8dd7-d333-46dd-9a45-f0e5841887c7", 1567374618);
            string Encryptdata = dataHashFilter.Response_AESDecrypt(data, "c870d479-3e55-4be8-afe0-d630c54b4ed0", "d1507484-2b13-49d3-8f6d-ae22ee90152c", 1567430854);
            Assert.AreEqual(data, Encryptdata);
            Assert.Pass();
        }
    }
}