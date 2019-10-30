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
            string data = "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";
            string decryptdata = dataHashFilter.AESDecrypt(data, "79e64a5a-7db7-4e3e-bf05-d2a8b453f6ee", "ac694805-2187-4903-b80d-c8c2d705eec0", 1571876310);
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


        [Test]
        public void Test144_decrypt()
        {
            string data = "anQHSGX4cytopko4ReE6hzjr3gtfUhY00TA1nSrzNEwBwcH/M2sCiFv0ewXmHgE/++om6gozUVFd7/vUaBJwWWrOVBB46YYY9x+KD0cK3yUu4I0gxFlVcVa6OyvMfrXwApd4/OVOCUq7M/BMB9i9ipD4sBDIHvNXeVu5MfERzbR6ji0A+HagtDJ1gJ9e1kIB1y9ROaAkoVDRM22YtCR5GuYwcD8jbyooc/TVkxI6TEP9nsWwmMeCCNfWZq+vFqDxFII0WzpL5QaIQznV69qeN/svaWSM2VnpwA/7xEk5oPaPTmBvDeRwJUdAl64N0ppgUo7s5u4cJ5ePFS5XYrwSdIpP9IBrNKWD36pPoXtjMpzDmR8saH1uRw4rEeYZtQApE7PApnKsbxEek7ADMYg+3vM6DceHREQAf/0WVOfFF9NNFNM4aFywo+Cnq40XYW0+vXmKkVspSIQnudzAQ89Tdqdxgr/06ODM09C51ezLmVs=";
            string decryptdata = dataHashFilter.Response_AESDecrypt_Android_144(data, "79e64a5a-7db7-4e3e-bf05-d2a8b453f6ee", "f0a1d83e-4de9-457d-9325-6e112dfda7d8", 6);
            //string Encryptdata = dataHashFilter.AESEncrypt_475(decryptdata, "", "",1567235435);
            //Assert.AreEqual(data, Encryptdata);
            Assert.Pass();
        }
    }
}