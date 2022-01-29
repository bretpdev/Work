#UTLWK19.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWK19.LWK19R1
then
rm ${reportdir}/ULWK19.LWK19R1
fi
if test -a ${reportdir}/ULWK19.LWK19R2
then
rm ${reportdir}/ULWK19.LWK19R2
fi
if test -a ${reportdir}/ULWK19.LWK19RZ
then
rm ${reportdir}/ULWK19.LWK19RZ
fi

# run the program

sas ${codedir}/UTLWK19.sas -log ${reportdir}/ULWK19.LWK19R1  -mautosource
