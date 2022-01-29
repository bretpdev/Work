#UTLWS45.jcl DEFAULT COLLECTION E-MAIL CAMPAIGN
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWS45.LWS45R1
then
rm ${reportdir}/ULWS45.LWS45R1
fi
if test -a ${reportdir}/ULWS45.LWS45RZ
then
rm ${reportdir}/ULWS45.LWS45RZ
fi
if test -a ${reportdir}/ULWS45.LWS45R2
then
rm ${reportdir}/ULWS45.LWS45R2
fi
if test -a ${reportdir}/ULWS45.LWS45R3
then
rm ${reportdir}/ULWS45.LWS45R3
fi
if test -a ${reportdir}/ULWS45.LWS45R4
then
rm ${reportdir}/ULWS45.LWS45R4
fi
if test -a ${reportdir}/ULWS45.LWS45R5
then
rm ${reportdir}/ULWS45.LWS45R5
fi

# run the program

sas ${codedir}/UTLWS45.sas -log ${reportdir}/ULWS45.LWS45R1  -mautosource
