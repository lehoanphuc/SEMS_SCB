﻿var temp;
var i = 0;
var k = 0;
function executeCommaDo(id) {    
    temp = document.getElementById(id).value;
    var arr = temp.split('.');
    temp = arr[0];
    var temp1 = "";
    if (arr[1] != null) temp1 = arr[1];
    for (i = 0; i < temp.length;i++) {
		for (k=i;k<temp.length;k++) {
			if (temp.charAt(k) == ',') {
				temp = temp.replace(',','');
			}
		}
	}

	var j = 0;
	var s = "";
	var s1 = "";
    var s2 = "";
	for (i=temp.length-1;i>=0;i--) {
		j = j+1;
		if (j == 3) {
			j = 0;
			s1 = temp.substring(0,i);
			s2 = temp.substring(i,i+3);
			s = "," + s2 + s;
		}
	}
	if (s1.length > 0) {
		s = s1 + s;
		document.getElementById(id).value = s;
	}else if (s.length > 0 && s2.length > 0){
		document.getElementById(id).value = s.substring(1,s.length);
    }
    temp = temp + temp1;
    
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    //var x = temp.split('.');
    //var x1 = x[0];
    //var x2 = x.length > 1 ? '.' + x[1] : '';
    //money = parseFloat(x1.replace(/,/g, ''));
    //document.getElementById(id).value = x2 === '' ? money.toLocaleString('mk-MK') : money.toLocaleString('mk-MK') + x2;
}

function executeCommaDo2(obj) {
	temp = obj.value;
	for (i=0;i<temp.length;i++) {
		for (k=i;k<temp.length;k++) {
			if (temp.charAt(k) == ',') {
				temp = temp.replace(',','');
			}
		}
	}

	var j = 0;
	var s = "";
	var s1 = "";
	var s2 = "";
	for (i=temp.length-1;i>=0;i--) {
		j = j+1;
		if (j == 3) {
			j = 0;
			s1 = temp.substring(0,i);
			s2 = temp.substring(i,i+3);
			s = "," + s2 + s;
		}
	}
	if (s1.length > 0) {
		s = s1 + s;
		obj.value = s;
	}else if (s.length > 0 && s2.length > 0){
		obj.value = s.substring(1,s.length);
	}
}

//function executeCommaDo3(money) {
//    var x = money.split('.');
//    var x1 = x[0];
//    var x2 = x.length > 1 ? '.' + x[1] : '';
//    money = parseFloat(x1.replace(/,/g, ''));
//    document.getElementById(id).value = x2 === '' ? money.toLocaleString('mk-MK') : money.toLocaleString('mk-MK') + x2;
//}

function executeComma(id,event) {
    if (document.getElementById(id).value.substring(0, 1) == "0" && document.getElementById(id).value.length>1)
	{	        
		document.getElementById(id).value = "";		
	}

   	else if ((event.keyCode >= 96 && event.keyCode <= 105)) {
		executeCommaDo(id);
	}
	else if (event.keyCode >= 48 && event.keyCode <= 57) {
		executeCommaDo(id);
	}
    else if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 19 || event.keyCode == 9 || event.keyCode == 190 || event.keyCode == 37 || event.keyCode == 39) {
		executeCommaDo(id);		
	}
	else if (detectmob()) {
	    executeCommaDo(id);
    } 
    else {
        alert("Please input number !");
        document.getElementById(id).value = "";	
	}
}

function executeComma2(id, event) {
    var item = document.getElementById(id);
    if (item.value.substring(0, 1) == "0" && item.value.length > 1) {
        item.value = "";
    }
    
    var reg = "/-?\d*\.?\d+/g";
    var tmp = item.value;
    var arr = tmp.match(reg);
    for (int i = 0; i < arr.length; i++) {
        tmp += arr[i];
    }
    executeCommaDo3(tmp);
}

function executeComma1(event, obj) {
	if ((event.keyCode >= 96 && event.keyCode <= 105)) {
		executeCommaDo2(obj);
	}
	else if (event.keyCode >= 48 && event.keyCode <= 57) {
		executeCommaDo2(obj);
	}
	else if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 19 || event.keyCode == 9) {
		executeCommaDo2(obj);
	}
	else if (detectmob()) {
	    executeCommaDo(id);
	}
	else {
		alert("Please input number !");
		obj.value = "";
	}
}

function detectmob() {
    if (navigator.userAgent.match(/Android/i))
    {
        return true;
    }
    else
    {
        return false;
    }
}
function replaceSQLChar(obj) {
    if (obj.value.length > 0) {
        obj.value = obj.value.replace(/'|!|#|\$|%|\^|&|\*/g, "");
    }
}