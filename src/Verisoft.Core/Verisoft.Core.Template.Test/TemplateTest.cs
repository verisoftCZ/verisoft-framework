using System;
using System.Collections.Generic;
using NUnit.Framework;
using Verisoft.Core.Template.Services;

namespace Verisoft.Core.Template.Test;

public class TemplateTest
{
    private ITemplateService _templateService;

    [SetUp]
    public void Setup()
    {
        _templateService = new TemplateService();
    }

    [Test]
    [TestCase("DataInput", "-DataInput-")]
    [TestCase("Loooooooooong inpuuuut", "-Loooooooooong inpuuuut-")]
    [TestCase(true, "-True-")]
    [TestCase(163, "-163-")]
    [TestCase(null, "--")]
    [TestCase("", "--")]
    public void ReplacingAllPlaceholdersSimple(object? data, string expected)
    {
        string template = "-{{data}}-";
        var res = _templateService.FillTemplate(template, new { data });
        Assert.That(res, Is.EqualTo(expected));
    }

    [Test]
    public void ReplacingPlaceholderDatetimeFormatWithDateOnly()
    {
        string template = "-{{date:dd.MM.yyyy}}-";
        string expected = "-02.01.2020-";

        object data = new { date = new DateTime(2020, 1, 2, 23, 23, 26) };
        var res = _templateService.FillTemplate(template, data);
        Assert.That(res, Is.EqualTo(expected));
    }
    
    [Test]
    public void ReplacingPlaceholderDatetimeFormatWithDateTime()
    {
        string template = "-{{date:dd.MM.yyyy HH:mm:ss}}-";
        string expected = "-02.01.2020 23:23:26-";

        object data = new { date = new DateTime(2020, 1, 2, 23, 23, 26) };
        var res = _templateService.FillTemplate(template, data);
        Assert.That(res, Is.EqualTo(expected));
    }

    [Test]
    public void ReplacingAllPlaceholdersComplex()
    {
        string template = """
                            <h1>{{Heading}}</h1>
                            <p>{{Buul}}</p>
                            <p>{{Numero}}</p>
                          """;
        string expected = """
                            <h1>Nadpis</h1>
                            <p>False</p>
                            <p>15903</p>
                          """;

        TestComplexClass data = new()
        {
            Heading = "Nadpis",
            Buul = false,
            Numero = 15903
        };
        var res = _templateService.FillTemplate(template, data);
        Assert.That(res, Is.EqualTo(expected));
    }

    [Test]
    public void ReplacingAllPlaceholdersWithNullValues()
    {
        string template = """
                            <h1>{{Heading}}</h1>
                            <p>{{Buul}}</p>
                            <p>{{Numero}}</p>
                          """;
        string expected = """
                            <h1></h1>
                            <p></p>
                            <p>15903</p>
                          """;

        TestComplexClass data = new()
        {
            Numero = 15903
        };
        var res = _templateService.FillTemplate(template, data);
        Assert.That(res, Is.EqualTo(expected));
    }

    [Test]
    public void ReplacingAllPlaceholdersList()
    {
        string template = """
                            <h1>{{Heading}}</h1>
                            <ul>
                                {{#each ListItems}}<li>{{Username}} - {{Index}}</li>
                                {{/each}}
                            </ul>
                          """;
        string expected = """
                            <h1>Nadpis</h1>
                            <ul>
                                <li>stankmi - 15</li>
                                <li>ppavel - 11</li>
                                <li>novakjan - 2399</li>
                                
                            </ul>
                          """;

        TestComplexClass data = new()
        {
            Heading = "Nadpis",
            ListItems = new List<TestComplexClassItem>
            {
                new()
                {
                    Username = "stankmi",
                    Index = 15
                },
                new()
                {
                    Username = "ppavel",
                    Index = 11
                },
                new()
                {
                    Username = "novakjan",
                    Index = 2399
                }
            }
        };
        var res = _templateService.FillTemplate(template, data);
        Assert.That(res, Is.EqualTo(expected));
    }

    [Test]
    public void ReplacingAllPlaceholdersDictionary()
    {
        string template = """
                            <h1>{{Heading}}</h1>
                            <ul>
                                {{#each Dict}}<li>{{Key:dd.MM.yyyy}} - {{Value}}</li>
                                {{/each}}
                            </ul>
                          """;
        string expected = """
                            <h1>Nadpis</h1>
                            <ul>
                                <li>02.01.2020 - 123</li>
                                <li>05.05.2019 - 312</li>
                                <li>05.11.2001 - 1</li>
                                <li>12.10.1995 - 0</li>
                                
                            </ul>
                          """;

        TestComplexClass data = new()
        {
            Heading = "Nadpis",
            Dict = new Dictionary<DateTime, int>()
            {
                [new DateTime(2020, 1, 2, 23, 23, 26)] = 123,
                [new DateTime(2019, 5, 5, 6, 26, 1)] = 312,
                [new DateTime(2001, 11, 5, 1, 56, 51)] = 1,
                [new DateTime(1995, 10, 12, 16, 54, 50)] = 0,
            }
        };
        var res = _templateService.FillTemplate(template, data);
        Assert.That(res, Is.EqualTo(expected));
    }

    [Test]
    public void ReplacingMissingKeyException()
    {
        string template = """
                            <h1>{{Heading}}</h1>
                            <h2>{{Subheading}}</h1>
                          """;
        TestComplexClass data = new() { Heading = "Nadpiis" };
        Assert.Throws<KeyNotFoundException>(() => _templateService.FillTemplate(template, data));
    }

    private class TestComplexClass
    {
        public string? Heading { get; set; }
        public DateTime? Date { get; set; }
        public bool? Buul { get; set; }
        public int? Numero { get; set; }
        public List<TestComplexClassItem>? ListItems { get; set; }
        public Dictionary<DateTime, int>? Dict { get; set; }
    }

    private class TestComplexClassItem
    {
        public string Username { get; set; }
        public int Index { get; set; }
    }
}