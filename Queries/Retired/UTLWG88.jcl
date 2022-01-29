#UTLWG88.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWG88.LWG88R1
then
rm ${reportdir}/ULWG88.LWG88R1
fi
if test -a ${reportdir}/ULWG88.LWG88R2
then
rm ${reportdir}/ULWG88.LWG88R2
fi
if test -a ${reportdir}/ULWG88.LWG88R3
then
rm ${reportdir}/ULWG88.LWG88R3
fi
if test -a ${reportdir}/ULWG88.LWG88R4
then
rm ${reportdir}/ULWG88.LWG88R4
fi
if test -a ${reportdir}/ULWG88.LWG88RZ
then
rm ${reportdir}/ULWG88.LWG88RZ
fi

# run the program

sas ${codedir}/UTLWG88.sas -log ${reportdir}/ULWG88.LWG88R1  -mautosource
