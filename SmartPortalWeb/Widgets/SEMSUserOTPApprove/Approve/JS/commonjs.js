﻿function validateEmpty(id,msg)
{
    if(document.getElementById(id).value=="")
    {
        window.alert(msg);
        return false;
    }
    else
    {
    return true;
    }
}
function validateMoney(id,msg)
{
    if(document.getElementById(id).value=="0" || document.getElementById(id).value=="")
    {
        window.alert(msg);
        return false;
    }
    else
    {
    return true;
    }
}
function IsNumeric(sText,aler)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;
   sText=document.getElementById(sText).value;
   
   if(sText!='')
   { 
       for (i = 0; i < sText.length && IsNumber == true; i++) 
          { 
          Char = sText.charAt(i); 
          if (ValidChars.indexOf(Char) == -1) 
             {
             IsNumber = false;
             window.alert(aler);
             return IsNumber;
             }
          }
       return IsNumber;
   }
   else
   {
    return true;
   }
   
   }
   
function checkEmail(emailID,aler) {
var email = document.getElementById(emailID);
var value=document.getElementById(emailID).value;
if(value!='')
{
    var filter = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$/;
    if (!filter.test(email.value)) {
    alert(aler);
    return false;
    }
    return true;
}
else
{
    return true;
}
}

function IsDateGreater(DateValue1, DateValue2,aler)
{

var DaysDiff;
DateValue1=document.getElementById(DateValue1).value;
DateValue2=document.getElementById(DateValue2).value;

Date1 = new Date(DateValue1);
Date2 = new Date(DateValue2);
DaysDiff = Math.floor((Date1.getTime() - Date2.getTime())/(1000*60*60*24));
if(DaysDiff > 0)
return true;
else
window.alert(aler);
return false;
}



