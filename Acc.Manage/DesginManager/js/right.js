function search()
{
	var searchID=document.getElementById("tblSearch");
	if(searchID!=null)
	{
		if(searchID.style.display=="none")
		{
			searchID.style.display="block";
			document.all("image2").src="../images/hidden.gif";
		}
		else
		{
			searchID.style.display="none";
			document.all("image2").src="../images/display.gif";
		}
	}
}
function search2()
{
	var searchID=document.getElementById("tblSearch");
	if(searchID!=null)
	{
		if(searchID.style.display=="none")
		{
			searchID.style.display="block";
			document.all("image2").src="../../images/hidden.gif";
		}
		else
		{
			searchID.style.display="none";
			document.all("image2").src="../../images/display.gif";
		}
	}
}
function searchParam2(ContentID,imageID)
{
	var searchID=document.getElementById(ContentID);
	if(searchID!=null)
	{
		if(searchID.style.display=="none")
		{
			searchID.style.display="block";
			document.getElementById(imageID).src="../../images/hidden.gif";
		}
		else
		{
			searchID.style.display="none";
			document.getElementById(imageID).src="../../images/display.gif";
		}
	}
}

//处理因UpdatePanel导致的样式在Firefox中出现的问题
function SetUpdatePanel(UpdatePanelID)
{
    document.getElementById(UpdatePanelID).style.height="100%";
    document.getElementById(UpdatePanelID).style.width="100%";
}