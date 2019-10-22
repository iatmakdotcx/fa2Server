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

        [Test]
        public void Test475()
        {
            string data = "{\"net_id\":\"933\",\"userDefR\":\"79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8\",\"sg_version\":\"475\",\"userHpR\":\"yCfH79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8R6X7C77A\",\"uuid\":\"34178f82-565a-442e-ae22-39b08c7d858d\",\"userAtkR\":\"iOC582ZAyCfH79K4K7bFvC00eUqp3BM799GoPlV5yP8k0R6SP7k3mrOpIKF9L5vuyOIFh23K8R6X7\",\"token\":\"6b371671-938b-46fd-96f7-3bc3f3195a23\"}";
            string decryptdata = dataHashFilter.AESEncrypt_475(data, "34178f82-565a-442e-ae22-39b08c7d858d", "6b371671-938b-46fd-96f7-3bc3f3195a23", 1571764163);
            string Encryptdata = dataHashFilter.AESDecrypt_475(decryptdata, "34178f82-565a-442e-ae22-39b08c7d858d", "6b371671-938b-46fd-96f7-3bc3f3195a23", 1571764163);
            //Assert.AreEqual(data, Encryptdata);
            Assert.Pass();
        }
        [Test]
        public void Test475_decrypt()
        {
            string data = "RYylfoBzZx0pwqO7814FDrhSAM4B6rPrasqjxV78Rl9lfm3GMPYN1VWYL9Wq9aQhi5t2wEWP5PNnS674m3dht13Wh7xbhdQ92xGXWkWjaIr18S7hVqpB97Y5Ip+WpqWV023G8lfQZVFN55HoCf1t33Lp+Ku+x7VqiLTkUw4WF14jkdVor27tv0inpKUHqskLsezw2R888X9d0BwR78rKMPzuBB3a/HHmsOFN81tJSGAJ6U19zSVk7sheM25MuJk4ANacpjIsyt3qJymr680JBHSzK2DzfKbrijX/lo9Jfl0zazVmLIbAQ2Fe5JzBCtK9Ci6/r6l1mnrK8EnIMg4OkXgyBRFo6cyXCujPWdv3y3VX0G0QFeJmtZK7YZXHSI6u/oPJiRfbLawNtjs4pgKoQ3OEezIz7rKUVvI6tMRElEBRXq/xmJCW1aocUREOh8u/uAxa2+qSXtJIyoLhA6GxectcIkWDaA6o8R/qQ56C/84ZeFpAdWAroVwr3zWe7sbEsslvhPim6dySC6PhBazQjwD+ez9qCTSWUlnBEga1P0F53UrhRzeA3+xgFHuvRO810mELKEuKMvhkiUVdiZ+uAneUngPNgzCoQfJx8IjhflTANzkNXIeyALRzktqxrM9VMCpdgDBWoSzlqkJrd1Bjn8DrUwbz9sjh1Kb9+xokVEo=";
            string decryptdata = dataHashFilter.AESDecrypt_475(data, "34178f82-565a-442e-ae22-39b08c7d858d", "6b371671-938b-46fd-96f7-3bc3f3195a23", 1571754174);
            //string Encryptdata = dataHashFilter.AESEncrypt_475(decryptdata, "", "",1567235435);
            //Assert.AreEqual(data, Encryptdata);
            Assert.Pass();
        }
    }
}