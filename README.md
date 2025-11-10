general info

this it the web api i developed in .net core for my website "language-engine".it is the backend of the website.it contains an excel file.this file contains 5 tables of 2 language proximity scores on the scale between 0.01 and 1,where 0.01 is as distant as it can be while 0.99 is as close as it can be while 1 is used for relationships between the same language ("French-French","Armenian-Armenian",etc).the 5 tables correspond to areas of knowledge of a language - Reading,Writing,Speaking,Listening and Grammar - meaning that the value in a the cell under the "Romanian" column and "German" row in the "Writing" table,same as under the "German" column and "Romanian" row,is the proximity between german and romanian in writing,how related the 2 writing systems are.each table is its own sheet.

-------------------------------------------------

how to call

the url of the api is https://language-engine-backend.onrender.com/api/proximity/query

the type of call is POST

it doesn't use any authentication or special headers or anything

in the json body,write the following:

{
    "fromLanguages":languageArray,
    "toLanguage":languageString
}

where "languageArray" is an array of strings and "languageString" is just a string.the strings in the array and the languageString must correspond to the columns in the excel file

--------------------------------------------------------------------

how to interpret results

you will receive a json response that looks like this:

[

    {
    
        "From": stringFromBodyArray,
        
        "To": languageString,
        
        "Reading": value of the 2 in the "Reading" sheet,
        
        "Writing": value of the 2 in the "Writing" sheet,
        
        "Speaking": value of the 2 in the "Speaking" sheet,
        
        "Listening": value of the 2 in the "Listening" sheet,
        
        "Grammar": value of the 2 in the "Grammar" sheet
        
    }
    
]

the numeric values you'll receive are the ones from the cells,with values between 0.01 and 1.you'll receive an array of objecs equal in length to the languageArray in the request body.each object in the response array will contain,in the "From" key the string from the request array element in the same index.the response array has the same order as the request array.

values below 0.2 are for very distant languages,those between 0.2 and 0.4 have some overlap,mostly historical,those above 0.75 are related languages within the same family of languages and those between 0.4 and 0.75 are moderately related.
