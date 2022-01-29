#UTLWS35.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWS35.LWS35R1
then
rm ${reportdir}/ULWS35.LWS35R1
fi
if test -a ${reportdir}/ULWS35.LWS35R2
then
rm ${reportdir}/ULWS35.LWS35R2
fi
if test -a ${reportdir}/ULWS35.LWS35R3
then
rm ${reportdir}/ULWS35.LWS35R3
fi
if test -a ${reportdir}/ULWS35.LWS35R4
then
rm ${reportdir}/ULWS35.LWS35R4
fi
if test -a ${reportdir}/ULWS35.LWS35R5
then
rm ${reportdir}/ULWS35.LWS35R5
fi
if test -a ${reportdir}/ULWS35.LWS35R6
then
rm ${reportdir}/ULWS35.LWS35R6
fi
if test -a ${reportdir}/ULWS35.LWS35R7
then
rm ${reportdir}/ULWS35.LWS35R7
fi
if test -a ${reportdir}/ULWS35.LWS35RZ
then
rm ${reportdir}/ULWS35.LWS35RZ
fi

# run the program

sas ${codedir}/UTLWS35.sas -log ${reportdir}/ULWS35.LWS35R1  -mautosource
