#UTNWR03.jcl 

#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing


if test -a ${reportdir}/UNWR03.NWR03R1
then
rm ${reportdir}/UNWR03.NWR03R1
fi
if test -a ${reportdir}/UNWR03.NWR03R2
then
rm ${reportdir}/UNWR03.NWR03R2
fi
if test -a ${reportdir}/UNWR03.NWR03R3
then
rm ${reportdir}/UNWR03.NWR03R3
fi

# run the program

sas ${codedir}/UTNWR03.sas -log ${reportdir}/UNWR03.NWR03R1  -mautosource
